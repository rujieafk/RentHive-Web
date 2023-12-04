namespace RentHive.Models
{
    public class UserDataGetter
    {
        //Tempholders
        public string ErrorMessage { get; set; }
        public string Temp_FirstName { get; set; }
        public string Temp_MiddleName { get; set; }
        public string Temp_LastName { get; set; }
        public string Temp_DisplayName { get; set; }
        public string Temp_Email { get; set; }


        //global use
        public int AdminID { get; set; } // this is a temporary holder
        public int Acc_id { get; set; }
        public int Rental_id { get; set; }
        public string Post_id { get; set; }
        public string response { get; set; }
        public int NumHolder { get; set; } // this is a temporary holder

        //account table
        //-------------start---------------------------
        public string Acc_FirstName { get; set; }
        public string Acc_LastName { get; set; }
        public string Acc_MiddleName { get; set; }
        public string Acc_DisplayName { get; set; }
        public string Acc_Birthdate { get; set; }
        public string Acc_PhoneNum { get; set; }
        public string Acc_Address { get; set; }
        public string Acc_Email { get; set; }
        public string Acc_Password { get; set; }
        public string Acc_UserType { get; set; }
        public int Acc_Active { get; set; }
        public string Acc_Ban { get; set; }
        public string Acc_BanDate { get; set; }
        public string Acc_BanEndDate { get; set; }
        public int Acc_Strikes { get; set; }

        public string userId { get; set; } // Selected User
        public string setTimeBan { get; set; }
        //-------------end---------------------------


        //picture table-------------------------------
        //------------------start--------------------------
        public string Images { get; set; }
        //------------------start--------------------------

        //rental table-------------------------------
        //------------------start--------------------------
        public string Rental_Details { get; set; }
        public string Rental_Size { get; set; }
        public string Rental_Category { get; set; }
        public string Rental_Specification { get; set; }
        public string Rental_Amenities { get; set; }
        public string Rental_Limit { get; set; }
        public string Rental_Location { get; set; }
        public string Rental_Conditions { get; set; }
        public string Rental_Tag { get; set; }
        public string Rental_Type { get; set; }
        public string Rental_Amount { get; set; }
        //------------------end--------------------------

        //post table
        //------------------start--------------------------
        public string Post_Title { get; set; }
        public string Post_RentPeriod { get; set; }
        public string Post_Term { get; set; }
        public string Post_Calendar { get; set; }
        public string Post_Status { get; set; }
        public string Post_Price { get; set; }
        public int Post_Active { get; set; }
        public string Post_BanDate { get; set; }
        public string Post_AdvDeposit { get; set; }
        public string Post_DatePosted { get; set; }
        //------------------end--------------------------

        //transaction table
        //------------------start--------------------------
        public int Trans_id { get; set; }
        public string Topic { get; set; }
        public string About { get; set; }
        public string PaymentAmount { get; set; }
        public string PaymentMode { get; set; }
        public string AdvDeposit { get; set; }
        public string Delivery { get; set; }
        public string TransDetails { get; set; }
        public string Status { get; set; }
        public string TransDate { get; set; }
        public string Date { get; set; }
        //------------------end--------------------------

        //userlog table
        //------------------start--------------------------
        public int ul_id { get; set; }
        public string ul_Timestamp { get; set; }
        public string ul_Origin { get; set; }
        public string ul_Action { get; set; }
        public string ul_SysResponse { get; set; }
        //------------------end--------------------------

        //report table
        //------------------start--------------------------
        public int Rep_id { get; set; }
        public string Rep_Topic { get; set; }
        public string Rep_Message { get; set; }
        public string Rep_Approve { get; set; }
        public string Rep_Date { get; set; }
        public string Reported_User { get; set; }
        //------------------end--------------------------

        //verification table
        //------------------start--------------------------
        
        public string Ver_id { get; set; }
        public string Ver_Status { get; set; }
        public string Ver_ApprovedBy { get; set; }
        public string Ver_DateSent { get; set; }
        //------------------end--------------------------
        

        //rent table
        //------------------start--------------------------
        
        public string Rent_id { get; set; }
        public string Rent_Status { get; set; }
        public string Rent_Dates { get; set; }
        public string Rent_DueDate { get; set; } 
        public string Rent_Delivery { get; set; }
        public string Rent_DeliveryStatus { get; set; }
        public string Rent_Quantity { get; set; }
        public string Rent_Inventory { get; set; }
        public string Rent_RenturnExtension { get; set; }
        public string Rent_DepositReturned { get; set; }
        //------------------end--------------------------


        //income table
        //------------------start--------------------------
        public string Income { get; set; }
        //------------------end--------------------------
    }
}
