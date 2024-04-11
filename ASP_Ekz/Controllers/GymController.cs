using ASP_Ekz.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ASP_Ekz.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GymController : ControllerBase
    {
        string conStr = @"Server=DESKTOP-S23LER7;Database=ASP_Ekz;Trusted_Connection=True;TrustServerCertificate=Yes;";
        //string conStr = @"Server=206-11\SQLEXPRESS;Database=ASP_Ekz;Trusted_Connection=True;TrustServerCertificate=Yes;";

        [HttpGet("AdminOrUser")]
        public ActionResult AdminOrUser(string email, string pwd)
        {
            using (SqlConnection db = new SqlConnection(conStr))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@email", email);
                parameters.Add("@pwd", pwd);
                parameters.Add("@userType", DbType.Int32, direction: ParameterDirection.Output);

                db.Execute("pIsAdmin", parameters, commandType: CommandType.StoredProcedure);

                int userType = parameters.Get<int>("@userType");

                switch (userType)
                {
                    case 1:
                        return Ok("Client");
                    case 2:
                        return Ok("Admin");
                    case 3:
                        return Ok("Trainer");
                    default:
                        return NotFound();
                }
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

                    SqlCommand cmd = new SqlCommand("pTraining", db);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", training.id);
                    cmd.Parameters.AddWithValue("@trainer_id", training.trainer_id);
                    cmd.Parameters.AddWithValue("@timeT_id", training.timeT_id);
                    cmd.Parameters.AddWithValue("@status_id", training.status_id);
                    cmd.Parameters.AddWithValue("@hall_id", training.hall_id);
                    cmd.Parameters.AddWithValue("@max_capacity", training.max_capacity);

                    SqlParameter resultParam = cmd.Parameters.Add("@result", SqlDbType.Int);
                    resultParam.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int result = Convert.ToInt32(resultParam.Value);

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
            using (SqlConnection db = new SqlConnection(conStr))
            {
                var result = -1;

                using (SqlCommand cmd = new SqlCommand("pTrainingAttendance", db))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@client_id", client_id);
                    cmd.Parameters.AddWithValue("@training_id", training_id);

                    db.Open();

                    result = cmd.ExecuteNonQuery();
                }

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

    }
}
