using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Geen.Core.Domains.Mentions.Repositories;
using Geen.Core.Domains.Replies.Repositories;
using Geen.Core.Interfaces.Common;

namespace Geen.Core.Domains.Mentions.Commands
{
    public class ReinitTextCommand : ICommand<Task>
    {
    }

    public class ReinitTextCommandDispatcher : ICommandDispatcher<ReinitTextCommand, Task>
    {
        private readonly IMentionRepository _mentionRepository;
        private readonly IReplyRepository _replyRepository;

        public ReinitTextCommandDispatcher(IMentionRepository mentionRepository, IReplyRepository replyRepository)
        {
            _mentionRepository = mentionRepository;
            _replyRepository = replyRepository;
        }

        public async Task Execute(ReinitTextCommand command)
        {
            var nicknames = await Nicknames();
            
            var mentions = await _mentionRepository.GetAll(nicknames.Count);

            var rnd = new Random(Environment.TickCount);
            
            foreach (var mention in mentions)
            {
                var mr = rnd.Next(0, nicknames.Count);

                await _mentionRepository.UpdateUser(mention.Id, nicknames[mr]);
            }
            
            var replies = await _replyRepository.GetAll(nicknames.Count);

            foreach (var reply in replies)
            {
                var rr = rnd.Next(0, nicknames.Count);

                await _replyRepository.UpdateUser(reply.Id, nicknames[rr]);
            }
        }

        private async Task<List<string>> Nicknames()
        {
            return new List<string>
            {
                "лучший из худших",
                "Чебурашка ниндзя",
                "cheLentano",
                "Вася Пупкин",
                "просто глюк",
                "lolkekchebyrek",
                "Злыдень с карданом",
                "Сладкий_чипС",
                "BOss_HockN",
                "White_Kristalik",
                "Просто_Бобрик",
                "Тётя Фанта",
                "papin_nocochek ",
                "МухаМор_СилаВик",
                "superkakawka",
                "Lol_for_all",
                "добрый жук",
                "Лысый аптекарь",
                "reach beach",
                "Печень Сталина",
                "Вежливый снайпер",
                "Tam_A_da",
                "Mad_cat",
                "MILBPUS",
                "Мемасян",
                "Стасян",
                "Славикт",
                "Хоа",
                "МистерМ",
                "Алголл",
                "Мисфо",
                "Крузерн",
                "Миха",
                "Саня",
                "Витя",
                "Axelof",
                "Alex",
                "Seeo",
                "Mika",
                "Mike",
                "Lesst",
                "Муравей",
                "НеТуда",
                "Добрый_Тролль",
                "Митяй",

            };
        }
    }
}
