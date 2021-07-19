using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;
using System.Collections.Generic;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();
        private static int j;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            var BMList = BreadmakerDb.Breadmakers;
            var allids = BMList.Select(s => s);
            List<string> Makersid = new List<string>();
           List<Mydata> Alist = new List<Mydata>();

            foreach (var id in allids)
            {
                Makersid.Add(id.BreadmakerId);
            }

            foreach(var ids in Makersid)
            {
                var reviews = BMList.Where(letter => letter.BreadmakerId == ids)
               .Select(rew => rew.Reviews);

                foreach(var eachrew in reviews)
                {
                    double count = 0.0; double numbers = 0.0;string titlehold = "";
                    for (int i = 0; i < eachrew.Count; i++)
                    {
                        count = count + eachrew[i].stars;
                        numbers++;
                    }
                    foreach (var idas in allids)
                    {
                        if (ids == idas.BreadmakerId)
                        {
                            titlehold = idas.title;
                        }
                    }
                    Mydata val = new Mydata();
                    val.Reviews=numbers;
                    val.Average= Math.Round(count/ numbers, 2);
                    val.Adjust= ratingAdjustmentService.Adjust(Math.Round(count / numbers, 2), numbers);
                    val.Title = titlehold;
                    Alist.Add(val);
                }

            }
           Alist = Alist
                  .OrderByDescending(Adjus => Adjus.Adjust)
                  .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
            Console.WriteLine(j + 1 + "\t" + Alist[j].Reviews + "\t" + Alist[j].Average + "\t" + Alist[j].Adjust + "\t" + Alist[j].Title);
        }
        }
    }
}
