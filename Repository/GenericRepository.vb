Imports Microsoft.EntityFrameworkCore
Imports System.Linq.Expressions

Public Class GenericRepository(Of TEntity As Class, TContext As DbContext)
    Implements IGenericRepository(Of TEntity)

    Protected ReadOnly DbContext As TContext
    Protected ReadOnly DbSet As DbSet(Of TEntity)

    Public Sub New(dbContext As TContext)
        Me.DbContext = dbContext
        Me.DbSet = dbContext.Set(Of TEntity)()
    End Sub

    Public Overridable Async Function GetAllAsync() As Task(Of IEnumerable(Of TEntity)) Implements IGenericRepository(Of TEntity).GetAllAsync
        Return Await DbSet.ToListAsync()
    End Function

    Public Overridable Async Function FindByConditionAsync(filter As Expression(Of Func(Of TEntity, Boolean))) As Task(Of IEnumerable(Of TEntity)) Implements IGenericRepository(Of TEntity).FindByConditionAsync
        Return Await DbSet.Where(filter).ToListAsync()
    End Function

    Public Overridable Async Function AddAsync(entity As TEntity) As Task(Of TEntity) Implements IGenericRepository(Of TEntity).AddAsync
        Await DbSet.AddAsync(entity)
        Return entity
    End Function

    Public Overridable Sub Update(entity As TEntity) Implements IGenericRepository(Of TEntity).Update
        DbSet.Update(entity)
    End Sub

    Public Overridable Sub Remove(entity As TEntity) Implements IGenericRepository(Of TEntity).Remove
        DbSet.Remove(entity)
    End Sub

    Public Overridable Async Function AddRangeAsync(entities As IEnumerable(Of TEntity)) As Task(Of IEnumerable(Of TEntity)) Implements IGenericRepository(Of TEntity).AddRangeAsync
        Await DbSet.AddRangeAsync(entities)
        Return entities
    End Function

End Class

Public Interface IGenericRepository(Of TEntity As Class)
    Function GetAllAsync() As Task(Of IEnumerable(Of TEntity))
    Function FindByConditionAsync(filter As Expression(Of Func(Of TEntity, Boolean))) As Task(Of IEnumerable(Of TEntity))
    Function AddAsync(entity As TEntity) As Task(Of TEntity)
    Function AddRangeAsync(entities As IEnumerable(Of TEntity)) As Task(Of IEnumerable(Of TEntity))
    Sub Update(entity As TEntity)
    Sub Remove(entity As TEntity)
End Interface
