using EFCAT.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Service {
    public abstract class AController<TEntity, TKey> : ControllerBase where TEntity : class {

        private IRepository<TEntity, TKey> _repository;

        public AController(IRepository<TEntity, TKey> repository) {
            _repository = repository;
        }

        //Method: Get
        //URL /entity/{id}
        [HttpGet("{id}")]
        public ActionResult<TEntity> Read([Required] TKey id) {
            var creature = _repository.Read(id);
            if (creature is null) return NotFound();
            return Ok(creature);
        }
        //Method: Get
        //URL /entity?start&count
        [HttpGet]
        public ActionResult<List<TEntity>> ReadAll(int start, int count) =>
            Ok(_repository.ReadAll(start, count));

        //Method: Post
        //URL /entity
        [HttpPost]
        public ActionResult<TEntity> Create([Required] TEntity e) => Ok(_repository.Create(e));

        //Method: Put
        //URL /entity/{id}
        [HttpPut("{id}")]
        public ActionResult Update([Required] TKey id, [Required] TEntity c) {
            var e = _repository.Read(id);
            if (e is null)return NotFound();
            _repository.Update(c);
            return NoContent();
        }

        //Method: Delete
        //URL /entity/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete([Required] TKey id) {
            var creature = _repository.Read(id);
            if (creature is null) return NotFound();
            _repository.Delete(creature);
            return NoContent();
        }
    }
}

