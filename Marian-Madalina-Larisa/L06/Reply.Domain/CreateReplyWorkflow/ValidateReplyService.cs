using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Text;
using static Reply.Domain.CreateReplyWorkflow.Reply;

namespace Reply.Domain.CreateReplyWorkflow
{
    public class ValidateReplyService
    {
        public Result<ValidatedReply> VerifyReply(UnvalidatedReply reply)
        {

           

            return new ValidatedReply(reply.Reply);
        }
    }
}
