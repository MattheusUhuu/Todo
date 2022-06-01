using Todo.Domain.Repositories;
using Todo.Domain.Commands;
using Todo.Domain.Commands.Contracts;
using Todo.Domain.Handlers.Contracts;
using Todo.Domain.Entities;
using Flunt.Notifications;

namespace Todo.Domain.Handlers;

public class TodoHandler : Notifiable,
    IHandler<CreateTodoCommand>,
    IHandler<UpdateTodoCommand>,
    IHandler<MarkTodoAsDone>,
    IHandler<MarkTodoAsUndone>
{
    private readonly ITodoRepository _repository;

    public TodoHandler(ITodoRepository repository)
    {
        _repository = repository;
    }

    public ICommandResult Handle(CreateTodoCommand command)
    {
        // Fail, fast, validation
        command.Validate();
        if (command.Invalid)
            return new GenericCommandResult(false, "Parece que sua tarefa esta errada", command.Notifications);

        // Gera o todoItem
        var todo = new TodoItem(command.Title, command.User, command.Date);

        // Salva no banco
        _repository.Create(todo);

        return new GenericCommandResult(true, "Tarefa salva", todo);
    }

    public ICommandResult Handle(UpdateTodoCommand command)
    {
        // Fail, fast, validation
        command.Validate();
        if (command.Invalid)
            return new GenericCommandResult(false, "Parece que sua tarefa está errada.", command.Notifications);

        // Recupera o todoItem (Reidratação)
        var todo = _repository.GetById(command.Id, command.User);

        // Atualiza o todoItem
        todo.UpdateTitle(command.Title);

        // Salva no banco
        _repository.Update(todo);

        // Retorno o resultado
        return new GenericCommandResult(true, "Tarefa Salva", todo);
    }

    public ICommandResult Handle(MarkTodoAsDone command)
    {
        // Fail, Fast, Validation
        command.Validate();
        if (command.Invalid)
            return new GenericCommandResult(false, "Ops, parece que sua tarefa está errada", command.Notifications);

        // Recupera o TodoItem
        var todo = _repository.GetById(command.Id, command.User);

        // Altera o estado
        todo.MarkAsDone();

        // Salva no banco
        _repository.Update(todo);

        // Retorna o TodoItem
        return new GenericCommandResult(true, "Tarefa salva", todo);
    }

    public ICommandResult Handle(MarkTodoAsUndone command)
    {
        // Fail, Fast, validation
        command.Validate();
        if (command.Invalid)
            return new GenericCommandResult(false, "Ops, parece que sua tarefa está errada", command.Notifications);

        // Recupera o todoItem
        var todo = _repository.GetById(command.Id, command.User);

        // Altera o estado do todo
        todo.MarkAsUndone();

        // Salva no banco
        _repository.Update(todo);

        // Retorna o todoItem
        return new GenericCommandResult(true, "Tarefa salva", todo);
    }
}
