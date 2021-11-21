﻿using EFCAT.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EFCAT.Service {
    public abstract class AControllerAsync<TEntity, TKey> : ControllerBase where TEntity : class {

        private IRepositoryAsync<TEntity, TKey> _repository;

        public AControllerAsync(IRepositoryAsync<TEntity, TKey> repository) {
            _repository = repository;
        }

        //Method: Get
        //URL /entity/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Read([Required] TKey id) {
            var creature = await _repository.ReadAsync(id);
            if (creature is null) return NotFound();
            return Ok(creature);
        }
        //Method: Get
        //URL /entity?start&count
        [HttpGet]
        public async Task<ActionResult<List<TEntity>>> ReadAll(int start, int count) =>
            Ok(await _repository.ReadAllAsync(start, count));

        //Method: Post
        //URL /entity
        [HttpPost]
        public async Task<ActionResult<TEntity>> Create([Required] TEntity e) => Ok(await _repository.CreateAsync(e));

        //Method: Put
        //URL /entity/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([Required] TKey id, [Required] TEntity c) {
            var e = await _repository.ReadAsync(id);
            if (e is null) return NotFound();
            await _repository.UpdateAsync(c);
            return NoContent();
        }

        //Method: Delete
        //URL /entity/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([Required] TKey id) {
            var creature = await _repository.ReadAsync(id);
            if (creature is null) return NotFound();
            await _repository.DeleteAsync(creature);
            return NoContent();
        }
    }
}

