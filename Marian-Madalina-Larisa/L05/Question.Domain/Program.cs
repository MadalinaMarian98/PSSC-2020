using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Question.Domain.CreateQuestionWorkflow;

namespace Test.App
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> taglist = new List<string>{ "HTML", "PHP" };
            var cmd = new CreateQuestionCmd("How to create a website", "How can i create a menu bar with php", taglist);
            var result = CreateQuestion(cmd);

            result.Match(
                    ProcessQuestionCreated,
                    ProcessQuestionNotCreated,
                    ProcessInvalidQuestion
                );

            Console.ReadLine();


            if (result.form == true)
            {
                Console.WriteLine("Would you like to give a vote?");
                Console.WriteLine("YES or NO?");
                string decision = Console.ReadLine();
                if (decision.Equals("Yes"))
                {
                    Console.WriteLine("Would you like to give a positive or negative vote ? ");
                    string vote = Console.ReadLine();
                    if (vote.Equals("P"))
                        result.getVotes(1);
                    else if (vote.Equals("N")) ;
                    result.getVotes(-1);
                }
            

            }
        }

        private static string ProcessQuestionNotCreated(CreateQuestionResult.QuestionNotAdded createQuestionResult)
        {
            Console.WriteLine("Question was not added. Error: " + createQuestionResult.ErrorMessage.ToString());
            
            return createQuestionResult.ErrorMessage.ToString();
        }

        private static string ProcessInvalidQuestion(CreateQuestionResult.QuestionValidationFailed questionValidation)
        {
            Console.WriteLine("failed validation: ");
            foreach(var error in questionValidation.ValidationErrros)
            {
                Console.WriteLine(questionValidation);
            }
            return questionValidation.ToString();
        }

        private static string ProcessQuestionCreated(CreateQuestionResult.QuestionAdded createQuestionResult)
        {
            Console.WriteLine("Id:" + createQuestionResult.Id);
            Console.WriteLine("Title: " + createQuestionResult.Title);
            Console.WriteLine("Body: " + createQuestionResult.Text);
            Console.WriteLine("Tags:");
            foreach(var item in createQuestionResult.Tags)
            {
                Console.WriteLine(item);
            }

            return createQuestionResult.ToString();
        }

        static CreateQuestionResult.IAskQuestionResult CreateQuestion(CreateQuestionCmd createQuestionCommand)
        {
            if(string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                string error = new string ("Title is empty!But it hasn't had to be");
                return new CreateQuestionResult.QuestionNotAdded(error);
            }
            if(createQuestionCommand.Title.Length<0 && !string.IsNullOrWhiteSpace(createQuestionCommand.Title))
            {
                    var errors=new List<string>() {" title must have more than 8 characters" };
                return new CreateQuestionResult.QuestionNotAdded(errors);
            }

            if(createQuestionCommand.Title.Length>1000)
                {
            var errors=new List<string>() ("Tile mustn't  be longer than 180 caracters");
                return new CreateQuestionResult.QuestionNotAdded(errors);

            }

            if(string.IsNullOrWhiteSpace(createQuestionCommand.Text))
            {
                string error = new string ("Body is empty!But it hasn't had to be");
                return new CreateQuestionResult.QuestionNotAdded(error);
            }

            if(createQuestionCommand.Text.Length<0 && !string.IsNullOrWhiteSpace(createQuestionCommand.Title))
                {
            var errors=new List<string>()("Body must have more than 10 characters");
            return new CreateQuestionResult.QuestionNotAdded(errors);
            }

            if(createQuestionCommand.Text.Length>1000)
                {
            var errors=new List<string>() ("Body must have just 1000 characters");
                return new CreateQuestionResult.QuestionNotAdded(errors);
            }

            if(createQuestionCommand.Tags.Length < 1)
                {
                var errors=new List<string>() ("Enter one tags");
                return new CreateQuestionResult.QuestionNotAdded(errors);

            }

            if(createQuestionCommand.Tags.Lenght > 3)
                {
            var errors=new List<string>() ("You entered too much tags,please delete some of them to have just 3");
            return new CreateQuestionResult.QuestionNotAdded(errors);
            }

             if(new Random().Next(10) > 7) 
            {
                var error = new List<string> {"Validation Failed"};
                return new CreateQuestionResult.QuestionValidationFailed(error);
            }


            var Id = Guid.NewGuid();
            var result = new CreateQuestionResult.QuestionAdded(Id, createQuestionCommand.Title, createQuestionCommand.Text, createQuestionCommand.Tags);
            return result;
        }
    }
}