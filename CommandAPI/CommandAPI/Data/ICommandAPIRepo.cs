using CommandAPI.Models;

namespace CommandAPI.Data;

public interface ICommandApiRepo
{
    bool SaveChange();
    IEnumerable<Command> GetAllCommands();
    Command GetCommandById(int id);
    void CreateCommand(Command cmd);
    void UpdateCommand(Command cmd);
    void DeleteCommand(Command cmd);

}