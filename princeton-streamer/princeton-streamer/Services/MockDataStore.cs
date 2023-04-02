using princeton_streamer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace princeton_streamer.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Modern Operating System", Description="Introduction" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Modern Operating System", Description="Processes And Threads" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Modern Operating System", Description="Memory Management" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Modern Operating System", Description="File Systems" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Introduction to Algorithms", Description="Foundations" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Introduction to Algorithms", Description="Data Structures" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Introduction to Algorithms", Description="Dynamic Programming" },
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}