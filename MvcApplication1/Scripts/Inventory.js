$(document).ready(function(){

	function get_notifications(){
		$.get('/api/inventory', function(data){
			var jsonData = JSON.parse(data);
			var output = '<h4>Expired Items</h4>';
			var expired = jsonData.ExpiredItems;
			var taken = jsonData.TakenItems;
			var inventory = jsonData.Inventory;
			
			$.each(expired, function(key, value) { 
				output += "<p>" + value.ID + " | " + value.Label + " | " + value.Type + " | " + value.ExpirationDate +  "</p>"  
			});

			output += '<h4>Items Taken From Inventory</h4>';

			$.each(taken, function(key, value) { 
				output += "<p>" + value.ID + " | "  + value.Label + " | " + value.Type + " | " + value.ExpirationDate +  "</p>"  
			});

			$('#notifications').html(output);

			output = '<h4>All Inventory</h4>';

			$.each(inventory, function(key, value) { 
				output += "<p>" + value.ID + " | "  + value.Label + " | " + value.Type + " | " + value.ExpirationDate +  "</p>"  
			});

			$('#inventory').html(output);



		}, 'json')
	}

	get_notifications();
})