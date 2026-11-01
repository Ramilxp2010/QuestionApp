using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionApp.Application.DataGenerator.Options
{
    public sealed class SeedOptions
    {
        public int QuestionCount { get; init; } = 1000;
        public int AnswerCount { get; init; } = 2000;
        public int UserCount { get; init; } = 100;
        public int RandomSeed { get; init; } = 12345;
    }
}
