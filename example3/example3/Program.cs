﻿using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
namespace example3
{
    class Program
    {
        static void Main(string[] args)
        {
            //test_insert();
            //test_mult_insert();
            //test_del();
            //test_mult_del();
            //test_update();
            //test_mult_update();
            //test_select_one(3);
            test_select_content_with_comment();
        }

        /// <summary>
        /// 测试插入单条数据
        /// </summary>
        static void test_insert()
        {
            var content = new Content
            {
                title = "标题3",
                content = "内容3",

            };
            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert：插入了{result}条数据！");
            }
        }
        /// <summary>
        /// 测试插入多条数据
        /// </summary>
        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    title = "批量标题6",
                    content="批量内容6"
                },
                  new Content
                {
                    title = "批量标题7",
                    content="批量内容7"
                },
                    new Content
                {
                    title = "批量标题8",
                    content="批量内容8"
                },
            };

            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"INSERT INTO [Content]
                (title, [content], status, add_time, modify_time)
VALUES   (@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert：插入了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试删除单条数据
        /// </summary>
        static void test_del()
        {
            var content = new Content
            {
                id = 2,

            };
            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"DELETE FROM [Content]
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_del：删除了{result}条数据！");
            }
        }

        /// <summary>
        /// 测试一次批量删除两条数据
        /// </summary>
        static void test_mult_del()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                id=1,

            },
               new Content
            {
                id=4,

            },
        };

            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"DELETE FROM [Content]
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_del：删除了{result}条数据！");
            }
        }


        /// <summary>
        /// 测试修改单条数据
        /// </summary>
        static void test_update()
        {
            var content = new Content
            {
                id = 5,
                title = "修改标题5",
                content = "修改内容5",

            };
            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"UPDATE  [Content]
SET         title = @title, [content] = @content, modify_time = GETDATE()
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_update：修改了{result}条数据！");
            }
        }


        /// <summary>
        /// 测试一次批量修改多条数据
        /// </summary>
        static void test_mult_update()
        {
            List<Content> contents = new List<Content>() {
               new Content
            {
                id=6,
                title = "批量修改标题6",
                content = "批量修改内容6",

            },
               new Content
            {
                id =7,
                title = "批量修改标题7",
                content = "批量修改内容7",

            },
        };

            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"UPDATE  [Content]
SET         title = @title, [content] = @content, modify_time = GETDATE()
WHERE   (id = @id)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_update：修改了{result}条数据！");
            }
        }

        /// <summary>
        /// 查询单条指定的数据
        /// </summary>
        static void test_select_one(int id)
        {
            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id=@id";
                var result = conn.QueryFirstOrDefault<Content>(sql_insert, new { id = id });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }
        /// <summary>
        /// 查询多条指定的数据
        /// </summary>
        static void test_select_list()
        {
            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from [dbo].[content] where id in @ids";
                var result = conn.Query<Content>(sql_insert, new { ids = new int[] { 6, 7 } });
                Console.WriteLine($"test_select_one：查到的数据为：");
            }
        }

        static void test_select_content_with_comment()
        {
            using (var conn = new SqlConnection("Data Source=(local);User ID=sa;Password=123456;Initial Catalog=test;Pooling=true;Max Pool Size=100;"))
            {
                string sql_insert = @"select * from content where id=@id;
select * from comment where content_id=@id;";
                using (var result = conn.QueryMultiple(sql_insert, new { id = 5 }))
                {
                    var content = result.ReadFirstOrDefault<ContentWithComment>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量{content.comments.Count()}");
                }

            }
        }


    }
}
