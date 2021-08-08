using System.Text;
using System.Threading.Tasks;
using Geen.Core.Domains.Clubs.Queries;
using Geen.Core.Domains.Mentions.Queries;
using Geen.Core.Domains.Players.Queries;
using Geen.Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Geen.Web.Application.Sitemap
{
    public class SitemapProvider
    {
        private readonly ILogger<SitemapProvider> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        private readonly IQueryDispatcher _queryDispatcher;

        public SitemapProvider(ILogger<SitemapProvider> logger, IQueryDispatcher queryDispatcher, 
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;    
            _queryDispatcher = queryDispatcher;
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<string> Generate()
        {
            _logger.LogInformation("SITEMAP REQUEST: {Ip}", _httpContextAccessor.HttpContext?.Request.Headers["X-Real-IP"]);
            
            var builder = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", 10 * 1024);

            builder.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            builder.AppendLine("<url>");
            builder.AppendLine("<loc>https://geen.one/clubs</loc>");
            builder.AppendLine("<changefreq>yearly</changefreq>");
            builder.AppendLine("</url>");

            var clubsTask =  _queryDispatcher.Execute(new ClubGetUrlsQuery());
            var playersTask = _queryDispatcher.Execute(new PlayerGetUrlsQuery());
            var mentionIdsTask = _queryDispatcher.Execute(new GetMentionIdentitiesQuery());
            
            await Task.WhenAll(clubsTask, playersTask, mentionIdsTask);

            foreach (var club in clubsTask.Result)
            {
                builder.AppendLine("<url>");
                builder.AppendLine($"<loc>https://geen.one/club/{club.UrlName}</loc>");
                builder.AppendLine("<changefreq>daily</changefreq>");
                builder.AppendLine("<priority>0.8</priority>");
                builder.AppendLine("</url>");

                if (!club.IsNational)
                {
                    builder.AppendLine("<url>");
                    builder.AppendLine($"<loc>https://geen.one/club/{club.UrlName}/players</loc>");
                    builder.AppendLine("<changefreq>monthly</changefreq>");
                    builder.AppendLine("<priority>0.8</priority>");
                    builder.AppendLine("</url>");
                }
            }

            foreach (var playerUrl in playersTask.Result)
            {
                builder.AppendLine("<url>");
                builder.AppendLine($"<loc>https://geen.one/player/{playerUrl}</loc>");
                builder.AppendLine("<changefreq>daily</changefreq>");
                builder.AppendLine("<priority>0.8</priority>");
                builder.AppendLine("</url>");
            }

            foreach (var mention in mentionIdsTask.Result)
            {
                builder.AppendLine("<url>");

                if (mention.Club != null)
                {
                    builder.AppendLine($"<loc>https://geen.one/club/{mention.Club.UrlName}/{mention.Id}</loc>");
                }
                else if (mention.Player != null)
                {
                    builder.AppendLine($"<loc>https://geen.one/player/{mention.Player.UrlName}/{mention.Id}</loc>");
                }
                else
                {
                   continue;
                }

                builder.AppendLine("<changefreq>daily</changefreq>");
                builder.AppendLine("<priority>1</priority>");
                builder.AppendLine("</url>");
            }


            builder.AppendLine("</urlset>");

            return builder.ToString();
        }
    }
}
