using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICrud.Data.Sql
{
   public  class ICrudCommand
   {
       ICrudConnectionString ConnectionString = new ICrudConnectionString();

       #region Insert
       public void Insert(string table,List<ICrudColumns> columns)
        {
            //Generate Sql command string
            var query = new StringBuilder();
            int x = 0;

            query.Append("Insert Into " + table + " (");
            foreach(var item in  columns)
            {
                x++;
               if(x == columns.Count)query.Append(item.Name);
               else query.Append(item.Name + ",");
               
            }
            query.Append(")Values(");
            
            x = 0;
            foreach (var item in columns)
            {
                x++;
                if (x == columns.Count) query.Append("@"+item.Name);
                else query.Append("@"+item.Name + ",");
            }
            query.Append(")");

            //Execute Sql command
           
            using(SqlConnection cn = new SqlConnection(ConnectionString.ConnectionString()))
            {
                using(SqlCommand cmd = new SqlCommand(query.ToString(),cn))
                {
                     foreach(var item in columns)
                     {
                         cmd.Parameters.Add(new SqlParameter("@"+item.Name, item.Value));
                     }
                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }

        }
       #endregion
       #region Select
       public DataTable Select(string table, int top = 0, List<ICrudColumns> select = null)
       {
            var dt = new DataTable();
            int x = 0;
           //Generate Sql Command
           var query = new StringBuilder();
           query.Append("Select ");
           if (top != 0) query.Append("Top " + top+" ");
           if (select == null) query.Append(" * ");
           else
           { 
               foreach(var item in select)
               {
                   x++;
                   if (x == select.Count) query.Append(item.Name);
                   else query.Append(item.Name+",");
                  
               }
           }
           query.Append(" From " + table);
          
           //Excute Sql Command
           using (SqlConnection cn = new SqlConnection(ConnectionString.ConnectionString()))
           {
               using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
               {
                 
                   using (SqlDataAdapter da = new SqlDataAdapter())
                   {
                       dt.Clear();
                       da.SelectCommand = cmd;
                       try
                       {
                           cn.Open();
                           da.Fill(dt);
                           cn.Close();
                       }
                       catch (Exception ex)
                       {
                           throw ex;
                       }
                   }

               }
           }
           return dt;
       }
       public DataTable SelectSearch(string table,List<ICrudColumns> search,int top,List<ICrudColumns> select = null)
           {
           var dt = new DataTable();
           int x = 0;
           //Generate Sql Command
           var query = new StringBuilder();
           query.Append("Select ");
           if (top != 0) query.Append("Top " + top + " ");
           if (select == null) query.Append(" * ");
           else
           {
               foreach (var item in select)
               {
                   x++;
                   if (x == select.Count) query.Append(item.Name);
                   else query.Append(item.Name + ",");

               }
           }
           query.Append(" From " + table + " ");
         
              
                   x = 0;
                   query.Append("Where ");
                   foreach (var item in search)
                   {
                       x++;
                       if (x == search.Count) query.Append(item.Name + " Like @" + item.Name);
                       else query.Append(item.Name + " Like @" + item.Name + " Or ");

                   }
           //Excute Sql Command
           using (SqlConnection cn = new SqlConnection(ConnectionString.ConnectionString()))
           {
               using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
               {
                   foreach(var item in search)
                   {
                       cmd.Parameters.Add(new SqlParameter("@" + item.Name, "%" + item.Value + "%"));
                   }
                   using (SqlDataAdapter da = new SqlDataAdapter())
                   {
                       dt.Clear();
                       da.SelectCommand = cmd;
                       try
                       {
                           cn.Open();
                           da.Fill(dt);
                           cn.Close();
                       }
                       catch (Exception ex)
                       {
                           throw ex;
                       }
                   }

               }
           }
           return dt;
       }
       public DataTable SelectFilter(string table, List<ICrudColumns> filter, int top, List<ICrudColumns> select = null)
       {
           var dt = new DataTable();
           int x = 0;
           //Generate Sql Command
           var query = new StringBuilder();
           query.Append("Select ");
           if (top != 0) query.Append("Top " + top + " ");
           if (select == null) query.Append(" * ");
           else
           {
               foreach (var item in select)
               {
                   x++;
                   if (x == select.Count) query.Append(item.Name);
                   else query.Append(item.Name + ",");

               }
           }
           query.Append(" From " + table + " ");


           x = 0;
           query.Append("Where ");
           foreach (var item in filter)
           {
               x++;
               if (x == search.Count) query.Append(item.Name + " = @" + item.Name);
               else query.Append(item.Name + " = @" + item.Name + " And ");

           }
           //Excute Sql Command
           using (SqlConnection cn = new SqlConnection(ConnectionString.ConnectionString()))
           {
               using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
               {
                   foreach (var item in filter)
                   {
                       cmd.Parameters.Add(new SqlParameter("@" + item.Name, item.Value));
                   }
                   using (SqlDataAdapter da = new SqlDataAdapter())
                   {
                       dt.Clear();
                       da.SelectCommand = cmd;
                       try
                       {
                           cn.Open();
                           da.Fill(dt);
                           cn.Close();
                       }
                       catch (Exception ex)
                       {
                           throw ex;
                       }
                   }

               }
           }
           return dt;
       }
       #endregion
       #region Update
       public void Update(string table, List<ICrudColumns> set, List<ICrudColumns> where)
       {
           //Generate Sql command string
           var query = new StringBuilder();
           int x = 0;

           query.Append("Update " + table + " Set ");
           foreach (var item in set)
           {
               x++;
               if (x == set.Count) query.Append(item.Name + " = @" + item.Name);
               else query.Append(item.Name + " = @" + item.Name + ",");

           }
           query.Append(" Where ");

           x = 0;
           foreach (var item in where)
           {
               x++;
               if (x == where.Count) query.Append(item.Name + " = @" + item.Name);
               else query.Append(item.Name + " = @" + item.Name + " And ");

           }

           //Execute Sql command

           using (SqlConnection cn = new SqlConnection(ConnectionString.ConnectionString()))
           {
               using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
               {
                   foreach (var item in set)
                   {
                       cmd.Parameters.Add(new SqlParameter("@" + item.Name, item.Value));
                   }
                   foreach (var item in where)
                   {
                       cmd.Parameters.Add(new SqlParameter("@" + item.Name, item.Value));
                   }
                   try
                   {
                       cn.Open();
                       cmd.ExecuteNonQuery();
                       cn.Close();
                   }
                   catch (Exception ex)
                   {
                       throw ex;
                   }
               }

           }
       }
       #endregion
       #region Delete
       public void Delete(string table,List<ICrudColumns> where)
       {
           //Generate Sql command string
           var query = new StringBuilder();
           int x = 0;

           query.Append("Delete From " + table);
          
           query.Append(" Where ");

           x = 0;
           foreach (var item in where)
           {
               x++;
               if (x == where.Count) query.Append(item.Name + " = @" + item.Name);
               else query.Append(item.Name + " = @" + item.Name + " And ");

           }

           //Execute Sql command

           using (SqlConnection cn = new SqlConnection(ConnectionString.ConnectionString()))
           {
               using (SqlCommand cmd = new SqlCommand(query.ToString(), cn))
               {
                  
                   foreach (var item in where)
                   {
                       cmd.Parameters.Add(new SqlParameter("@" + item.Name, item.Value));
                   }
                   try
                   {
                       cn.Open();
                       cmd.ExecuteNonQuery();
                       cn.Close();
                   }
                   catch (Exception ex)
                   {
                       throw ex;
                   }
               }

           }
       }
       #endregion
   }
}
