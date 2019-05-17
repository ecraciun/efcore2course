using ContentPlatform.Data;
using Microsoft.EntityFrameworkCore;
using System;
using Console = System.Console;

namespace ContentPlatform.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            using(var ctx = new ContentPlatformContext())
            {
                ctx.Database.Migrate();
            }
        }
    }
}