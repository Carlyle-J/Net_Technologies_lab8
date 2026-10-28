using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Product_Inventory_Management_Model.Models;

namespace Product_Inventory_Management_Model.Controllers
{
    public class ProductController : Controller
    {
        string cs = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Inventorydb;Integrated Security=True";

        // GET: Product
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT ProductId, ProductName, Quantity, Price FROM Products", con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"])
                    };
                    products.Add(product);
                }
            }
            return View(products);
        }

        // GET: Product/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Products (ProductName, Quantity, Price) VALUES (@ProductName, @Quantity, @Price)", con);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // GET: Product/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product p = null;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT ProductId, ProductName, Quantity, Price FROM Products WHERE ProductId = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    p = new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    };
                }
            }
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        // POST: Product/Edit
        [HttpPost]
        public ActionResult Edit(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE Products SET ProductName = @ProductName, Quantity = @Quantity, Price = @Price WHERE ProductId = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductName", p.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", p.Quantity);
                cmd.Parameters.AddWithValue("@Price", p.Price);
                cmd.Parameters.AddWithValue("@ProductId", p.ProductId);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE ProductId = @ProductId", con);
                cmd.Parameters.AddWithValue("@ProductId", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}