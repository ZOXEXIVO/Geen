using System;
using System.Linq;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Repositories;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Domains.Players.Repositories;
using Geen.Core.Domains.Users;
using Geen.Core.Interfaces.Common;
using Geen.Core.Models.Content.Extensions;
using Geen.Core.Models.Mentions;
using Geen.Core.Services.Text;

namespace Geen.Core.Domains.Mentions.Commands;

public record MentionCreateCommand : ICommand<Task<MentionModel>>
{
    public MentionCreateModel Model { get; set; }
    public UserModel User { get; set; }
}

public class MentionCreateCommandDispatcher : ICommandDispatcher<MentionCreateCommand, Task<MentionModel>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IContentService _contentService;
    private readonly IMentionRepository _mentionRepository;
    private readonly IPlayerRepository _playerRepository;

    private readonly ITextService _textService;

    public MentionCreateCommandDispatcher(
        IMentionRepository mentionRepository,
        ITextService textService,
        IContentService contentService,
        IPlayerRepository playerRepository,
        IClubRepository clubRepository)
    {
        _mentionRepository = mentionRepository;
        _textService = textService;
        _contentService = contentService;
        _playerRepository = playerRepository;
        _clubRepository = clubRepository;
    }

    public async Task<MentionModel> Execute(MentionCreateCommand command)
    {
        var mention = new MentionModel
        {
            User = command.User,
            Date = DateTime.UtcNow,
            IsApproved = _textService.CanApproveMention(command.Model.Text)
        };

        if (command.Model.PlayerId.HasValue)
        {
            mention.Player = await _playerRepository.GetById(command.Model.PlayerId.Value);

            if (mention.IsApproved) await _playerRepository.IncrementMentionsCount(command.Model.PlayerId.Value);
        }

        if (command.Model.ClubId.HasValue) mention.Club = await _clubRepository.GetById(command.Model.ClubId.Value);

        var processingInfo = _contentService.Process(command.Model.Text, mention.ToContentContext());

        mention.Text = processingInfo.Text;

        mention.Related.Players = processingInfo.PlayerIds.ToList();
        mention.Related.Clubs = processingInfo.ClubIds.ToList();

        mention.Title = _contentService.GenerateBasicTitle(mention);

        await _mentionRepository.Save(mention);

        return mention;
    }
}