using ASP_Ekz.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Nodes;

namespace ASP_Ekz.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GymController : ControllerBase
    {
        string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_Ekz;Trusted_Connection=True;TrustServerCertificate=Yes;";
        //string conStr = @"Server=206-11\SQLEXPRESS;Database=ASP_Ekz;Trusted_Connection=True;TrustServerCertificate=Yes;";

        [HttpPost("AdminOrUser")]
        public ActionResult AdminOrUser(AdminOrUser model)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(conStr))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@email", model.email);
                    parameters.Add("@pwd", model.pwd);
                    parameters.Add("@role_id", DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@user_id", DbType.Int32, direction: ParameterDirection.Output);

                    db.Execute("pIsAdmin", parameters, commandType: CommandType.StoredProcedure);

                    int role_id = parameters.Get<int>("@role_id");
                    int user_id = parameters.Get<int>("@user_id");

                    switch (role_id)
                    {
                        case 1:
                            return Ok(new { userType = "Client", user_id = user_id });
                        case 2:
                            return Ok(new { userType = "Admin", user_id = user_id });
                        case 3:
                            return Ok(new { userType = "Trainer", user_id = user_id });
                        default:
                            return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpGet("GetAllOrTrainerIdOrHallId")]
        public ActionResult GetAllOrTrainerIdOrHallId(string hallOrTrId)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Training>("pTraining;2", new { hallOrTrId }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpPost("AddOrUpdateTraining")]
        public ActionResult AddOrUpdateTraining([FromBody] Training2 training)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(conStr))
                {
                    db.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", training.id);
                    parameters.Add("@trainer_id", training.trainer_id);
                    parameters.Add("@timeT_id", training.timeT_id);
                    parameters.Add("@status_id", training.status_id);
                    parameters.Add("@hall_id", training.hall_id);
                    parameters.Add("@max_capacity", training.max_capacity);
                    parameters.Add("@result", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    db.Execute("pTraining", parameters, commandType: CommandType.StoredProcedure);

                    int result = parameters.Get<int>("@result");

                    var response = new { result = result };

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpGet("DeleteTraining")]
        public ActionResult DeleteTraining(int training_id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Training2>("pTraining;3", new { training_id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("GetAllOrTrainerFio")]
        public ActionResult GetAllOrTrainerFio(string fio)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Trainer>("pTrainer;2", new { fio }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpPost("AddOrUpdateTrainer")]
        public ActionResult AddOrUpdateTrainer([FromBody] Trainer2 trainer)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(conStr))
                {
                    db.Open();

                    SqlCommand cmd = new SqlCommand("pTrainer", db);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@trainer_id", trainer.trainer_id);
                    cmd.Parameters.AddWithValue("@fio", trainer.fio);
                    cmd.Parameters.AddWithValue("@dateBirth", trainer.dateBirth);
                    cmd.Parameters.AddWithValue("@number", trainer.number);
                    cmd.Parameters.AddWithValue("@gender", trainer.gender);
                    cmd.Parameters.AddWithValue("@category_id", trainer.category_id);
                    cmd.Parameters.AddWithValue("@special_id", trainer.special_id);
                    cmd.Parameters.AddWithValue("@email", trainer.email);
                    cmd.Parameters.AddWithValue("@pwd", trainer.pwd);
                    cmd.Parameters.AddWithValue("@role_id", trainer.role_id);

                    cmd.ExecuteNonQuery();

                    return Ok("Trainer added or updated successfully");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpGet("DeleteTrainer")]
        public ActionResult DeleteTrainer(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Trainer2>("pTrainer;3", new { id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("GetAllOrClientFio")]
        public ActionResult GetAllOrClientFio(string fio)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Client>("pClient;2", new { fio }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpPost("AddOrUpdateClient")]
        public ActionResult AddOrUpdateClient([FromBody] Trainer2 trainer)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(conStr))
                {
                    db.Open();

                    SqlCommand cmd = new SqlCommand("pClient", db);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@trainer_id", trainer.trainer_id);
                    cmd.Parameters.AddWithValue("@fio", trainer.fio);
                    cmd.Parameters.AddWithValue("@dateBirth", trainer.dateBirth);
                    cmd.Parameters.AddWithValue("@number", trainer.number);
                    cmd.Parameters.AddWithValue("@gender", trainer.gender);
                    cmd.Parameters.AddWithValue("@category_id", trainer.category_id);
                    cmd.Parameters.AddWithValue("@special_id", trainer.special_id);
                    cmd.Parameters.AddWithValue("@email", trainer.email);
                    cmd.Parameters.AddWithValue("@pwd", trainer.pwd);
                    cmd.Parameters.AddWithValue("@role_id", trainer.role_id);

                    cmd.ExecuteNonQuery();

                    return Ok("Trainer added or updated successfully");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpGet("DeleteClient")]
        public ActionResult DeleteClient(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<Client2>("pClient;3", new { id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("TrainingAttendance")]
        public ActionResult TrainingAttendance(int client_id, int training_id)
        {
            try
            {
                using (SqlConnection db = new SqlConnection(conStr))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@client_id", client_id);
                    parameters.Add("@training_id", training_id);

                    db.Open();
                    int result = db.Execute("pTrainingAttendance", parameters, commandType: CommandType.StoredProcedure);

                    switch (result)
                    {
                        case 0:
                            return Ok("Вы успешно записались на тренировку!");
                        case -1:
                            return BadRequest("Ошибка: тренировка не существует!");
                        case -2:
                            return BadRequest("Ошибка: все места на тренировке заняты!");
                        case -3:
                            return BadRequest("Ошибка: места на тренировке ограничены и уже заняты!");
                        case -4:
                            return BadRequest("Ошибка при обработке запроса!");
                        default:
                            return BadRequest("Неизвестная ошибка!");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request: " + ex.Message);
            }
        }

        [HttpGet("GetTrainingAttendance")]
        public IActionResult GetTrainingAttendance(int user_id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<TrainingAttendance>("pTrainingAttendance;2", new { user_id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }

        [HttpGet("DeleteTrainingAttendance")]
        public ActionResult DeleteTrainingAttendance(int id)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var res = db.Query<TrainingAttendance>("pTrainingAttendance;3", new { id }, commandType: CommandType.StoredProcedure);
                return Ok(res);
            }
        }
    }
}
