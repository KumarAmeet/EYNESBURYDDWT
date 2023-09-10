using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace Assignment2.Controllers
{
    public class HomeController : Controller
    {
        string constr = ConfigurationManager.ConnectionStrings["FoodDB"].ConnectionString;
        public ActionResult Index()
        {
            ViewBag.Title = "Food Deliver Report";
            DataSet ds = new DataSet();
            string constr = ConfigurationManager.ConnectionStrings["FoodDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "select Top(20)o.order_id,o.order_status,o.order_amount, c.channel_name from orders o inner join channels c on c.channel_id = o.channel_id";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                }
            }
            return View(ds);
        }

        public ActionResult Details()
        {
            ViewBag.Title = "Deliver Details";
            return View();
        }

        public ActionResult OrderHubs()
        {
            ViewBag.Title = "Hub Order Report";
            return View();
        }

        public ActionResult Payment()
        {
            ViewBag.Title = "Payment Analysis";
            return View();
        }
        public ActionResult ListFilteredOrders(string FilterBy, string txtSearch)
        {
            DataTable ds = new DataTable();
            JsonResult result = new JsonResult();

            using (SqlConnection con = new SqlConnection(constr))
            {
               using (SqlCommand cmd = new SqlCommand("SelectFilteredOrders", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FilterBy", SqlDbType.VarChar, 500)).Value = FilterBy;
                    cmd.Parameters.Add(new SqlParameter("@txtSearch", SqlDbType.VarChar, 500)).Value = txtSearch;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                    result = this.Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                    return result;
                }
            }            
        }

        public ActionResult ListFilteredPayements(string grouping,string paymentmethod)
        {
            DataTable ds = new DataTable();
            JsonResult result = new JsonResult();

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SelectFilteredPayements", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the command
                    cmd.Parameters.Add(new SqlParameter("@paymentmethod", SqlDbType.VarChar, 500)).Value = paymentmethod;
                    cmd.Parameters.Add(new SqlParameter("@grouping", SqlDbType.VarChar, 500)).Value = grouping;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                    result = this.Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                    return result;
                }
            }
        }

        public ActionResult ListFilteredHubs()
        {
            DataTable ds = new DataTable();
            JsonResult result = new JsonResult();

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SelectFilteredHubs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(ds);
                    }
                    result = this.Json(JsonConvert.SerializeObject(ds), JsonRequestBehavior.AllowGet);
                    return result;
                }
            }
        }

        public ActionResult DeleteOrders(string paymentorderid)
        {
            DataTable ds = new DataTable();
            JsonResult result = new JsonResult();

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();

                // Define your SQL query to delete the payment based on payment_order_id
                string deleteQuery = "DELETE FROM Payments WHERE payment_order_id = @PaymentOrderId; delete from orders where order_id = @paymentorderid;";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                {
                    // Set the parameter for payment_order_id
                    cmd.Parameters.AddWithValue("@PaymentOrderId", paymentorderid);

                    // Execute the SQL command to delete the payment
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Return a success response
                        return Json(new { success = true, message = "Payment deleted successfully" });
                    }
                    else
                    {
                        // Return an error response
                        return Json(new { success = false, message = "Payment not found or deletion failed" });
                    }
                }
            }
        }

        public ActionResult UpdateOrder(string OrderId, string OrderStatus, string OrderAmount)
        {
            DataTable ds = new DataTable();
            JsonResult result = new JsonResult();

            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                string deleteQuery = "Update Orders Set order_status = @OrderStatus, order_amount = @OrderAmount  WHERE order_id = @OrderId;";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, con))
                {
                    // Set the parameter for payment_order_id
                    cmd.Parameters.AddWithValue("@OrderId", OrderId);
                    cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
                    cmd.Parameters.AddWithValue("@OrderAmount", OrderAmount);

                    // Execute the SQL command to delete the payment
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Return a success response
                        return Json(new { success = true, message = "Order Updated successfully" });
                    }
                    else
                    {
                        // Return an error response
                        return Json(new { success = false, message = "Order not found or updation failed" });
                    }
                }
            }
        }
    }
}