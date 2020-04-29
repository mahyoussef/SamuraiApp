using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace SamuraiUI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        static void Main(string[] args)
        {
            InsertSamurai();
            InsertMultipleSamurais();
            SimpleSamuraiQuery();
            MoreQueries();
            RetrieveAndUpdateSamurai();
            RetrieveAndUpdateMultipleSamurais();
            InsertBattle();
            QueryAndUpdateBattleDisconnected();
            DeleteSamurai();
            DeleteSamuraiById(3);
            InsertSamuraiWithQuotes();
            InsertQuote(1);
            EagerLoadingSamuraiWithQuotes();
            ProjectionSamuraiWithQuotes();
        }

        private static void ProjectionSamuraiWithQuotes()
        {
            var samurai = _context.Samurais.Select(s => new
            {
                samurai = s,
                quotes = s.Quotes.Where(q => q.Text.Contains("sword"))
            }).ToList();
        }
        private static void EagerLoadingSamuraiWithQuotes()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            foreach (var quote in samurai.Quotes)
            {
                Console.WriteLine(quote.Text);
            }
        }
        private static void InsertQuote(int samuraiId)
        {
            var quote = new Quote
            {
                Text = "Hey Watch out my thunder sword !",
                SamuraiId = samuraiId
            };
            _context.Quotes.Add(quote);
            _context.SaveChanges();
        }
        private static void InsertSamuraiWithQuotes()
        {
            var samurai = new Samurai()
            {
                Name = "XinZhao",
                Quotes = new List<Quote>
                {
                    new Quote {Text = "Watch out for my sharp sword !"},
                    new Quote {Text = "I told you, Watch out for my sharp sword !"},
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void DeleteSamuraiById(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            _context.Remove(samurai);
            _context.SaveChanges();
        }
        private static void DeleteSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            _context.Remove(samurai);
            _context.SaveChanges();
        }
        private static void QueryAndUpdateBattleDisconnected()
        {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using (var context = new SamuraiContext())
            {
                context.Battles.Update(battle);
                context.SaveChanges();
            }
        }
        private static void InsertBattle()
        {
            var battle = new Battle
            {
                Name = "Battle of Okehazma",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15),
            };
            _context.Battles.Add(battle);
            _context.SaveChanges();
        }
        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }
        private static void RetrieveAndUpdateSamurai()
        {   
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }
        private static void MoreQueries()
        {
            var samuraiName = "Julie";
            var samurais = _context.Samurais.Where(s => s.Name == samuraiName).ToList();
            var samuraisQuerySyntax = (from s in _context.Samurais
                                       where s.Name == samuraiName
                                       select s).ToList();
            Console.WriteLine("From LINQ Method Syntax");
            foreach (var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
            Console.WriteLine("From LINQ Query Syntax");
            foreach (var samurai in samuraisQuerySyntax)
            {
                Console.WriteLine(samurai.Name);
            }
        }
        private static void SimpleSamuraiQuery()
        {
            var samurais = _context.Samurais.ToList();
            var samuraisQuerySyntax = (from s in _context.Samurais
                                      select s).ToList();

            Console.WriteLine("From LINQ Method Syntax");
            foreach (var samurai in samurais)
                Console.WriteLine(samurai.Name);

            Console.WriteLine("From LINQ Query Syntax");
            foreach(var samurai in samuraisQuerySyntax)
                Console.WriteLine(samurai.Name);
        }
        private static void InsertMultipleSamurais()
        {
            var samurais = new List<Samurai>() 
            {   new Samurai { Name = "Sampson" },
                new Samurai { Name = "Jack" },
            };
            _context.Samurais.AddRange(samurais);
            _context.SaveChanges();
        }
        private static void InsertSamurai()
        {
            var samurai = new Samurai { Name = "Julie" };
            using (var context = new SamuraiContext())
            {   
                context.Samurais.Add(samurai);
                context.SaveChanges();
                Console.WriteLine("here");
            }
        }
    }
}
