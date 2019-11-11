using System;
using Geen.Core.Domains.Players;

namespace Geen.Core.Domains.Votes
{
    public class VoteModel
    {
        public string Id { get; set; }

        public int? LeftPlayerId { get; set; }
        public int? RightPlayerId { get; set; }
        
        public int? WinnerId { get; set; }

        public DateTime Date {get;set;}
    }
    
    public class VoteFullModel
    {
        public PlayerModel Left { get; set; }
        public PlayerModel Right{ get; set; }
    }
}