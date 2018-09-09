using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiDirect.Flow
{
    internal class SaveStep : FlowStep
    {
        private readonly Func<Task<IActionResult>> _saveFunc;

        public SaveStep(Func<Task<IActionResult>> saveFunc)
        {
            _saveFunc = saveFunc;
        }

        public override async Task<IActionResult> Execute()
        {
            return await _saveFunc();
        }
    }
}
