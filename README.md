# InventorySystem
A basic inventory system in .Net

Running the application:

1) Using Visual Studion 2013 and .NET framework 4.5 or greater open the solution file. 

2) Run the application.

3) On the home page you can find a description of the api and how to use it.

4) On the home page you can also find a form to test adding an item. The home page also displays data as it was retrieved using JQUERY Ajax calls to the api get inventory method. 

5) Notifications: I was unclear about what was required for notifications. In an effort to save time I included expired and taken items as a part of the inventory get api call. I would have created a separate controller for notifications if I had more time.  

Trade-offs:

1) Security - I would have typically required an active session and user login validation before allowing access to the API. The api methods would all check to see if there is an active user session that passed login validation before returning data.
The encrpyted (md5+salt) user password would be stored in a users database for login validation.  

2) Database - I relied on MVC models to perform database routines. With heavier queries I would have constructed them by hand for performance gains. 

3) Tests: I ran out of time to implement tests for this API. 
