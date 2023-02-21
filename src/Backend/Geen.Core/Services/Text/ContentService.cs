using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Geen.Core.Domains.Mentions;
using Geen.Core.Models.Content;
using Geen.Core.Services.Interfaces;
using Geen.Core.Services.Text.Extensions;

namespace Geen.Core.Services.Text;

public interface IContentService
{
    ContentProcessingResult Process(string text, ContentContext context);
    string GenerateBasicTitle(MentionModel mention);
}

public class ContentService : IContentService
{
    private readonly IClubCacheRepository _clubCacheRepository;
    private readonly IPlayerCacheRepository _playerCacheRepository;

    public ContentService(IPlayerCacheRepository playerCacheRepository,
        IClubCacheRepository clubCacheRepository)
    {
        _playerCacheRepository = playerCacheRepository;
        _clubCacheRepository = clubCacheRepository;
    }

    public ContentProcessingResult Process(string text, ContentContext context)
    {
        var cleanedText = text.Trim().AsText();

        var builder = new StringBuilder(cleanedText);

        var words = cleanedText
            .Split(new[] { ' ', ',', '.', '(', ')', '\n', '-', '/', '\\', '<', '>' },
                StringSplitOptions.RemoveEmptyEntries)
            .Where(x => x.Length > 3);

        var result = new ContentProcessingResult();

        var replacedWords = new HashSet<string>();

        foreach (var word in words)
        {
            if (replacedWords.Contains(word))
                continue;

            var loweredWord = word.ToLower();

            var playerInfo = _playerCacheRepository.GetPlayerUrl(loweredWord, context);
            if (!string.IsNullOrWhiteSpace(playerInfo.Url))
            {
                builder.Replace(word, $"<a href=\"/player/{playerInfo.Url}\" target=\"blank\">{word}</a>");
                result.PlayerIds.Add(playerInfo.Id);

                replacedWords.Add(word);
            }

            var clubInfo = _clubCacheRepository.GetClubUrl(loweredWord, context);
            if (!string.IsNullOrWhiteSpace(clubInfo.Url))
            {
                builder.Replace(word, $"<a href=\"/club/{clubInfo.Url}\" target=\"blank\">{word}</a>");
                result.ClubIds.Add(clubInfo.Id);

                replacedWords.Add(word);
            }
        }

        result.Text = builder.ToString();

        return result;
    }

    public string GenerateBasicTitle(MentionModel mention)
    {
        if (mention.Player != null) return GenerateTitleForPlayer(mention);

        if (mention.Club != null) return GenerateTitleForClub(mention);

        return string.Empty;
    }

    private string GenerateTitleForPlayer(MentionModel playerMention)
    {
        return ExtractPhraze(playerMention.Text);
    }

    private string GenerateTitleForClub(MentionModel clubMention)
    {
        return ExtractPhraze(clubMention.Text);
    }

    private string ExtractPhraze(string text)
    {
        var cleanText = StripTextFromTags(text);

        var wordSplittedText = cleanText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (wordSplittedText.Length > 9)
            return ClearTailFromNonAplha(string.Join(" ", wordSplittedText.Take(9)));

        if (char.IsLower(cleanText.First()))
            return MakeFirstLetterUppercase(cleanText);

        return ClearTailFromNonAplha(cleanText);
    }

    private static string MakeFirstLetterUppercase(string text)
    {
        return text.First().ToString().ToUpper() + text.Substring(1);
    }

    private static string ClearTailFromNonAplha(string text)
    {
        var lastAlphaIndex = text.Length - 1;

        while (lastAlphaIndex >= 1 && !char.IsLetterOrDigit(text[lastAlphaIndex]))
            lastAlphaIndex--;

        return text.Substring(0, lastAlphaIndex + 1);
    }

    private string StripTextFromTags(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var s = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);

        s = s.Replace("&nbsp;", " ");

        s = Regex.Replace(s, @"\s+", " ");
        s = Regex.Replace(s, @"\r\n", " ");
        s = Regex.Replace(s, @"\n+", " ");

        return s;
    }
}

public class ContentProcessingResult
{
    public ContentProcessingResult()
    {
        PlayerIds = new SortedSet<int>();
        ClubIds = new SortedSet<int>();
    }

    public string Text { get; set; }

    public SortedSet<int> PlayerIds { get; set; }
    public SortedSet<int> ClubIds { get; set; }
}