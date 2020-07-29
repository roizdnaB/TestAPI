using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo repository;
        private readonly IMapper mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDTO>> GetAllCommands()
        {
            var commandItems = repository.GetAllCommands();

            return Ok(mapper.Map<IEnumerable<CommandReadDTO>>(commandItems));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDTO> GetCommandById(int id)
        {
            var commandItem = repository.GetCommandById(id);
            if (commandItem == null)
                return NotFound();

            return Ok(mapper.Map<CommandReadDTO>(commandItem));
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommand(CommandCreateDTO commandCreateDTO)
        {
            var commandModel = mapper.Map<Command>(commandCreateDTO);
            repository.CreateCommand(commandModel);
            repository.SaveChanges();

            var commandReadDTO = mapper.Map<CommandReadDTO>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDTO.Id }, commandReadDTO);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandCreateDTO commandUpdateDTO)
        {
            var commandModelFromRepo = repository.GetCommandById(id);
            if (commandModelFromRepo == null)
                return NotFound();

            mapper.Map(commandUpdateDTO, commandModelFromRepo);

            repository.UpdateCommand(commandModelFromRepo);
            repository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandCreateDTO> patchDoc)
        {
            var commandModelFromRepo = repository.GetCommandById(id);
            if (commandModelFromRepo == null)
                return NotFound();

            var commandToPatch = mapper.Map<CommandCreateDTO>(commandModelFromRepo);

            patchDoc.ApplyTo(commandToPatch, ModelState);
            if (!TryValidateModel(commandToPatch))
                return ValidationProblem(ModelState);

            mapper.Map(commandToPatch, commandModelFromRepo);

            repository.UpdateCommand(commandModelFromRepo);
            repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = repository.GetCommandById(id);
            if (commandModelFromRepo == null)
                return NotFound();

            repository.DeleteCommand(commandModelFromRepo);
            repository.SaveChanges();

            return NoContent();
        }
    }
}