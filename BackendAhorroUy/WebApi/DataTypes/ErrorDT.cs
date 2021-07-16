using BusinessLogicException;
using DomainException;
using RepositoryException;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.DataTypes
{   [ExcludeFromCodeCoverage]
    public class ErrorDT
    {
        public string ErrorCode { get; set; }
        public int Status { get; set; }
        public string Details { get; set; }

        public ErrorDT(Exception ex)
        {
            if (ex is UserException or DomainBusinessLogicException or ClientBusinessLogicException)
            {
                Status = 400;
                string msg;
                switch (ex.Message)
                {
                    //User
                    case "ERR_USER_NAME_EMPTY":
                        msg = "User.Name can not be null or empty";
                        Status = 404;
                        break;
                    case "ERR_USER_USERNAME_EMPTY":
                        msg = "User.Username can not be null or empty";
                        break;
                    case "ERR_USER_PASSWORD_EMPTY":
                        msg = "User.Password cannot be null or empty";
                        break;
                    case "ERR_USER_FIELDS_EMPTY":
                        msg = "name, username, password fields are obligatory";
                        break;
                    case "ERR_USER_USERNAME_ALREADY_EXISTS":
                        msg = "Already exists a user with this username.";
                        break;
                    case "ERR_USERS_GET_INCORRECT":
                        msg = "User not found in the database";
                        break;
                    //Session
                    case "ERR_LOGIN_INCORRECT_CREDENTIALS":
                        msg = "Incorrect username or password";
                        break;
                    case "ERR_LOGOUT_TOKEN_NULL_NOT_FOUND":
                        msg = "Login token invalid or not found";
                        break;
                    case "ERR_AUTH_MISSING":
                        msg = "Missing Auth token in header";
                        Status = 401;
                        break;
                    case "ERR_AUTH_INVALID":
                        msg = "Invalid Auth header";
                        Status = 401;
                        break;
                    case "ERR_AUTH_INCORRECT":
                        msg = "Auth header not found";
                        Status = 401;
                        break;
                    //Coupons
                    case "ERR_COUPONS_USERNAME_INCORRECT_NOT_FOUND":
                        msg = "Incorrect Username";
                        break;
                    //Category
                    case "ERR_CATEGORIES_NOT_FOUND":
                        msg = "Not found categories";
                        Status = 404;
                        break;
                    //ProductMarket
                    case "ERR_PRODUCTS_NOT_FOUND":
                        msg = "Not found products";
                        Status = 404;
                        break;
                    case "ERR_PRODUCTS_WITH_DISCOUNTS_NOT_FOUND":
                        msg = "Not found products with discounts";
                        Status = 404;
                        break;
                    case "ERR_PRODUCTS_BY_SEARCH_NOT_FOUND":
                        msg = "Not found products by the search done";
                        Status = 404;
                        break;
                    case "ERR_PRODUCT_SEARCHED_BY_BARCODE_NOT_FOUND":
                        msg = "Not found product by the barcode";
                        Status = 404;
                        break;
                    case "ERR_NOT_FOUND_MIN_AND_MAX_PRICE":
                        msg = "Not found min and max price for the product searched";
                        Status = 404;
                        break;
                    //DATABASE
                    case "ERR_CAN_NOT_CONNECT_DATABASE": 
                        msg = "An error ocurred when try established connection with the database";
                        Status = 500;
                        break; 
                    //BestOption
                    case "ERR_OBTEINED_BEST_OPTION":
                        Status = 404;
                        msg = "Not found options for the products and quantities selected";
                        break;
                    //Purchases
                    case "ERR_FOUNDING_USER_TO_SAVE_PURCHASE":
                        Status = 404;
                        msg = "Not found user for saving the purchase"; 
                        break; 

                    default:
                        msg = ex.Message;
                        break;

                }
                ErrorCode = ex.Message;
                Details = msg;
            }
            else
            {
                if (ex is ServerException)
                {
                    
                    ErrorCode = ex.Message;
                    Status = 500;
                    Details = "An error ocurred when try established connection with the database";
                    
                }
            }
        }
    }
}