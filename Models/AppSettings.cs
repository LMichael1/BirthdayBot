using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayBot.Models
{
    //TODO: Remove Tokens
    public static class AppSettings
    {
        public static string Url { get; set; } = "https://birthdayreminderbot.herokuapp.com/";
        public static string Name { get; set; } = "pzbirthdayreminderbot";
        public static string Key { get; set; } = "1021020710:AAGMfdIs22y0rTwc5a6GynMOb81DNuYEYWI";
    }
}
