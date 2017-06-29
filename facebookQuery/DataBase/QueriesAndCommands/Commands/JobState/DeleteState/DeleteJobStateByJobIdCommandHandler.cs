using System;
using System.Linq;
using DataBase.Context;

namespace DataBase.QueriesAndCommands.Commands.JobState.DeleteState
{
    public class DeleteJobStateByJobIdCommandHandler : ICommandHandler<DeleteJobStateByJobIdCommand, VoidCommandResponse>
    {
        private readonly DataBaseContext _context;

        public DeleteJobStateByJobIdCommandHandler()
        {
            _context = new DataBaseContext();
        }

        public VoidCommandResponse Handle(DeleteJobStateByJobIdCommand command)
        {
            try
            {
                var stateModel = _context.JobsState.FirstOrDefault(model => model.JobId == command.JobId);

                if (stateModel != null) _context.JobsState.Remove(stateModel);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
            }

            return new VoidCommandResponse();
        }
    }
}
