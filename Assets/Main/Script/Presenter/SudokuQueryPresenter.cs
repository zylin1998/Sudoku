using System;
using System.Collections;
using System.Collections.Generic;
using Loyufei;
using Loyufei.DomainEvents;

namespace Sudoku
{
    public class SudokuQueryPresenter
    {
        public SudokuQueryPresenter(SudokuQueryModel model, DomainEventService service) 
        {
            Model = model;

            service.Register<Query>   (Query, Declarations.Sudoku);
            service.Register<QueryAll>(Query, Declarations.Sudoku);
        }

        public SudokuQueryModel Model { get; }

        public void Query(Query query) 
        {
            query?.Response.Invoke(Model.Query(query.Id));
        }

        public void Query(QueryAll query)
        {
            query?.Response.Invoke(Model.QueryAll());
        }
    }

    public class Query : DomainEventBase 
    {
        public Query(int id, Action<int> response)
        {
            Id       = id;
            Response = response;
        }

        public int         Id       { get; }
        public Action<int> Response { get; }
    }

    public class QueryAll : DomainEventBase
    {
        public QueryAll(Action<IEnumerable<IEntity<int>>> response)
        {
            Response = response;
        }

        public Action<IEnumerable<IEntity<int>>> Response { get; }
    }
}