$(document).ready(function(){

	function get_inventory(){
		$.get('/api/inventory', function(data){
			var jsonData = JSON.parse(data);
			var output = '<h4>Expired Items</h4>';
			var inventory = jsonData.Inventory;
			
			output = '<h4>Inventory</h4>';

			$.each(inventory, function(key, value) { 
				output += "<p>" + value.ID + " | "  + value.Label + " | " + value.Type + " | " + value.ExpirationDate +  "</p>"  
			});

			$('#inventory').html(output);



		}, 'json')
	}

	if (!!window.EventSource) {
	   var source = new EventSource('http://localhost:62168/api/notifications/');
	   source.addEventListener('message', function (e) {
	        var json = JSON.parse(e.data);
	        $('#notifications').append(e.data);
	   }, false);

	   source.addEventListener('open', function (e) {
	     
	   }, false);

	  source.addEventListener('error', function (e) {
	    if (e.readyState == EventSource.CLOSED) {
	         console.log("error!");
	    }
	  }, false);
	} else {
	  // not supported!
	  //fallback to something else
	}

	$('form').submit(function(){
		
		$.post('api/inventory', $(this).serialize(), function(res) {
            	get_inventory();
          });

		return false;
	})


	get_inventory();
})