using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class SqlCommanderRepo : ICommanderRepo
    {
        private readonly CommanderContext context;

        public SqlCommanderRepo(CommanderContext context)
        {
            this.context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            context.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if (cmd == null)
                throw new ArgumentNullException(nameof(cmd));

            context.Commands.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands() => context.Commands.ToList();

        public Command GetCommandById(int id) => context.Commands.FirstOrDefault(x => x.Id == id);

        public bool SaveChanges() => (context.SaveChanges() >= 0);

        public void UpdateCommand(Command cmd)
        {
        }
    }
}