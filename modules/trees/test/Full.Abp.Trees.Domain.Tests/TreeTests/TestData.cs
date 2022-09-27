using System.Collections.Generic;

namespace Full.Abp.TreeStructure.TreeTests;

public class TestData
{
    public static List<Category> GetList()
    {
        var list = new List<Category>() {
            new Category() { Id = 1 },
            new Category() { Id = 2 },
            new Category() { Id = 3 },
            new Category() { Id = 11, ParentId = 1 },
            new Category() { Id = 12, ParentId = 1 },
            new Category() { Id = 13, ParentId = 1 },
            new Category() { Id = 111, ParentId = 11 },
            new Category() { Id = 112, ParentId = 11},
            new Category() { Id = 121, ParentId = 12 },
            new Category() { Id = 21, ParentId = 2 },
            new Category() { Id = 211, ParentId = 21 },
            new Category() { Id = 2111, ParentId = 211 },
            new Category() { Id = 22, ParentId = 2 },
            new Category() { Id = 221, ParentId = 22 },
            new Category() { Id = 222, ParentId = 22 },
            new Category() { Id = 2221, ParentId = 222 },
            new Category() { Id = 31, ParentId = 3 },
            new Category() { Id = 32, ParentId = 3 },
            new Category() { Id = 33, ParentId = 3 },
            new Category() { Id = 331, ParentId = 33 }
        };


        return list;
    }
}