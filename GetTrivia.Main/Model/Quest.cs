﻿namespace GetTrivia.ConsoleService.Model
{
    public class Quest
    {
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }
        public string Question { get; set; }
    }
}