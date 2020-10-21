using System;
using System.Collections.Generic;
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

            if(string.IsNullOrWhiteSpace(createQuestionCommand.Text))
            {
                string error = new string ("Body is empty!But it hasn't had to be");
                return new CreateQuestionResult.QuestionNotAdded(error);
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