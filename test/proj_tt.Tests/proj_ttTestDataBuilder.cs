using proj_tt.EntityFrameworkCore;
using proj_tt.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace proj_tt.Tests
{
    public class TestDataBuilder
    {
        private readonly proj_ttDbContext _context;
        public TestDataBuilder(proj_ttDbContext context)
        {
            _context = context;
        }
        public void Build()
        {
            _context.Tasks.AddRange(new Task("Follow the white rabbit", "Follow the white rabbit in order to know the reality."), new Task("Clean your room")
            {
                State = TaskState.Completed
            });
        }

    }
}
