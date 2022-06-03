using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Todo.Domain.Entities;
using Todo.Domain.Queries;

namespace Todo.Domain.Tests.QueryTests;

[TestClass]
public class TodoQueryTests
{
    private List<TodoItem> _items;

    public TodoQueryTests()
    {
        _items = new List<TodoItem>();
        _items.Add(new TodoItem("Tarefa 1", "Usuario1", DateTime.Now));
        _items.Add(new TodoItem("Tarefa 2", "Usuario1", DateTime.Now));
        _items.Add(new TodoItem("Tarefa 3", "PedroMatheus", DateTime.Now));
        _items.Add(new TodoItem("Tarefa 4", "Usuario", DateTime.Now));
        _items.Add(new TodoItem("Tarefa 5", "PedroMatheus", DateTime.Now));
    }

    [TestMethod]
    public void Dada_a_consulta_retornar_tarefas_apenas_do_usuario_PedroMatheus()
    {
        var result = _items.AsQueryable().Where(TodoQueries.GetAll("PedroMatheus"));
        Assert.AreEqual(2, result.Count());
    }
}
