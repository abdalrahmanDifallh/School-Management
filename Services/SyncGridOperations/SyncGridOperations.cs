
using Core;
using Core.Domains;

using Microsoft.EntityFrameworkCore;

using Syncfusion.EJ2.Base;

namespace Services.SyncGridOperations
{
    static public class SyncGridOperations<T> where T : class
    {

        public static async Task<PagedListResult<T>> PagingAndFilterAsync(IQueryable<T> query, DataManagerRequest dm)
        {
            try
            {
                var count = 0;
                if (dm != null)
                {
                    DataOperations operation = new();
                    if (dm.Search != null && dm.Search.Count > 0)
                    {
                        query = operation.PerformSearching(query, dm.Search);  //Search
                    }
                    if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
                    {
                        query = operation.PerformSorting(query, dm.Sorted);
                    }
                    if (dm.Where != null && dm.Where.Count > 0) //Filtering
                    {
                        foreach (var where in dm.Where)
                        {
                            //if (where.predicates != null)
                            //{
                            //    //foreach (var predicate in where?.predicates)
                            //    //{
                            //    //    if (predicate.Field == null)
                            //    //    {
                            //    //        predicate.Field = predicate.predicates?.FirstOrDefault().Field;
                            //    //    }
                            //    //    var actualName = typeof(T).GetProperties()
                            //    //    .FirstOrDefault(a => a.Name.ToLower() == predicate.Field.ToLower())?
                            //    //    .Name;

                            //    //    predicate.Field = actualName;
                            //    //    actual2 = predicate.Field;
                            //    //}
                            //    where?.predicates.ForEach(predicate =>
                            //    {
                            //        if (predicate.Field == null)
                            //        {
                            //            predicate.Field = where.predicates?.FirstOrDefault().Field;
                            //        }
                            //        var actualName = typeof(T).GetProperties()
                            //        .FirstOrDefault(a => a.Name.ToLower() == predicate.Field?.ToLower())?
                            //        .Name;

                            //        predicate.Field = actualName;
                            //    });
                            //}

                            //if (!string.IsNullOrEmpty(where.Field))
                            //{
                            //    var actualName = typeof(T).GetProperties()
                            //       .FirstOrDefault(a => a.Name.ToLower() == where.Field.ToLower())?
                            //       .Name;

                            //    where.Field = actualName;
                            //}

                            dm.Where = GetActualPropertyName(dm.Where);

                            try
                            {
                                query = operation.PerformFiltering(query, dm.Where, where.Condition);
                            }
                            catch (Exception ex)
                            { // ignore
                            }
                        }

                    }

                    count = await query.CountAsync();
                    if (dm.Skip != 0)
                    {
                        query = operation.PerformSkip(query, dm.Skip);   //Paging
                    }
                    if (dm.Take != 0)
                    {
                        query = operation.PerformTake(query, dm.Take);
                    }
                }

                return new PagedListResult<T>
                {
                    Result = await query.ToListAsync(),
                    Count = count,
                };
            }
            catch (Exception ex)
            {
                //TODO Log
                return null;
            }

        }
        private static List<WhereFilter> GetActualPropertyName(List<WhereFilter> predicates)
        {
            if (predicates == null)
            {
                return predicates;
            }

            foreach (var predicate in predicates)
            {
                if (predicate.Field == null)
                {
                    GetActualPropertyName(predicate.predicates);
                }
                else
                {
                    var actualName = typeof(T).GetProperties()
                .FirstOrDefault(a => a.Name.ToLower() == predicate.Field.ToLower())?
                .Name;

                    predicate.Field = actualName;

                }

                //GetActualPropertyName(predicate.predicates);
            }
            return predicates;
        }

    }
}
