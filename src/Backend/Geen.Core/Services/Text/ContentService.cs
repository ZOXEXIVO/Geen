using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Geen.Core.Domains.Mentions;
using Geen.Core.Models.Content;
using Geen.Core.Services.Interfaces;

namespace Geen.Core.Services.Text;

public interface IContentService
{
    ContentProcessingResult Process(string text, ContentContext context);
    string GenerateBasicTitle(MentionModel mention);
}

public partial class ContentService : IContentService
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
        var cleanedText = text.AsSpan().Trim().ToString();

        var words = cleanedText.Split(' ', ',', '.', '(', ')', '\n', '-', '/', '\\', '<', '>')
            .Where(x => x.Length > 3);

        var result = new ContentProcessingResult();

        var replacedWords = new HashSet<string>();

        var playerReplacements = new Dictionary<string, string>();
        var clubReplacements = new Dictionary<string, string>();

        foreach (var word in words)
        {
            if (replacedWords.Contains(word))
                continue;

            var loweredWord = word.ToString().ToLowerInvariant();

            var playerInfo = _playerCacheRepository.GetPlayerUrl(loweredWord, context);
            if (!string.IsNullOrWhiteSpace(playerInfo.Url))
            {
                playerReplacements[word.ToString()] = $"<a href=\"/player/{playerInfo.Url}\" target=\"blank\">{word}</a>";
                result.PlayerIds.Add(playerInfo.Id);
                replacedWords.Add(word.ToString());
            }

            var clubInfo = _clubCacheRepository.GetClubUrl(loweredWord, context);
            if (!string.IsNullOrWhiteSpace(clubInfo.Url))
            {
                clubReplacements[word.ToString()] = $"<a href=\"/club/{clubInfo.Url}\" target=\"blank\">{word}</a>";
                result.ClubIds.Add(clubInfo.Id);
                replacedWords.Add(word.ToString());
            }
        }

        var pattern = string.Join("|", playerReplacements.Keys.Concat(clubReplacements.Keys).Select(Regex.Escape));
        result.Text = Regex.Replace(cleanedText, pattern, match =>
        {
            if (playerReplacements.TryGetValue(match.Value, out var playerReplacement))
                return playerReplacement;
            if (clubReplacements.TryGetValue(match.Value, out var clubReplacement))
                return clubReplacement;
            return match.Value;
        });

        return result;
    }

    public string GenerateBasicTitle(MentionModel mention) =>
        mention.Player != null ? GenerateTitleForPlayer(mention) :
        mention.Club != null ? GenerateTitleForClub(mention) : string.Empty;

    private string GenerateTitleForPlayer(MentionModel playerMention) => ExtractPhrase(playerMention.Text);

    private string GenerateTitleForClub(MentionModel clubMention) => ExtractPhrase(clubMention.Text);

    private string ExtractPhrase(string text)
    {
        var cleanText = StripTextFromTags(text);
        var wordSplittedText = cleanText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return wordSplittedText.Length > 9 ? ClearTailFromNonAlpha(string.Concat(wordSplittedText.Take(9))) :
            char.IsLower(cleanText[0]) ? char.ToUpperInvariant(cleanText[0]) + cleanText[1..] : ClearTailFromNonAlpha(cleanText);
    }

    private static string ClearTailFromNonAlpha(string text)
    {
        var span = text.AsSpan();
        var lastAlphaIndex = span.Length - 1;

        while (lastAlphaIndex >= 1 && !char.IsLetterOrDigit(span[lastAlphaIndex]))
            lastAlphaIndex--;

        return span[..(lastAlphaIndex + 1)].ToString();
    }

    private static readonly Regex TagRegex = MyTagRegex();
    private static readonly Regex WhitespaceRegex = MyWhitespaceRegex();
    private static readonly Regex NewlineRegex = MyNewlineRegex();
    private static readonly Regex MultiNewlineRegex = MyMultiNewlineRegex();

    private string StripTextFromTags(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var stripped = TagRegex.Replace(text, string.Empty).Replace("&nbsp;", " ");
        stripped = WhitespaceRegex.Replace(stripped, " ");
        stripped = NewlineRegex.Replace(stripped, " ");
        stripped = MultiNewlineRegex.Replace(stripped, " ");

        return stripped;
    }

    [GeneratedRegex(@"<(.|\n)*?>", RegexOptions.Compiled)]
    private static partial Regex MyTagRegex();
    [GeneratedRegex(@"\s+", RegexOptions.Compiled)]
    private static partial Regex MyWhitespaceRegex();
    [GeneratedRegex(@"\r\n", RegexOptions.Compiled)]
    private static partial Regex MyNewlineRegex();
    [GeneratedRegex(@"\n+", RegexOptions.Compiled)]
    private static partial Regex MyMultiNewlineRegex();
}

public class ContentProcessingResult
{
    public ContentProcessingResult()
    {
        PlayerIds = new HashSet<int>();
        ClubIds = new HashSet<int>(); 
    }

    public string Text { get; set; }

    public HashSet<int> PlayerIds { get; set; }
    public HashSet<int> ClubIds { get; set; }
}