let key
let value
function getUpcomingArrivals(part) {
	console.log(part)
	$.ajax({
		url: `/User/UpcomingArrivals`
	}).then(function (data) {
		createATableFromTheData(data, part, false)
	}).fail(
		function (err) {
			console.error(err)
		}
	)
}
function searchForArrival(part, filter, search) {
	console.log(`${filter},${search}`)
	let query = createQuery(filter, search)
	console.log(`query is: ${query}`)
	if (query == "all") {
		getUpcomingArrivals(part)
	}
	else if (query != "error") {
		key = query[0]
		value = query[1]
		console.log(key)
		console.log(value)
		$.ajax({
			url: `/User/SearchUpcomingArrivals/params?key=${key}&value=${value}`
		}).then(function (data) {
			if (data !== undefined) {
				console.log(data[0])
				console.log(`${part}`)
				console.log(`ofir`)
				createATableFromTheData(data, part, true)
			}
			else {
				$("#tblUpcomingArrivals tbody").remove();
				$("#pagePagination li").remove();
			}
		}).fail(
			function (err) {
				console.error(err)
			}
		)
	}
	else {
		$("#tblUpcomingArrivals tbody").remove();
		$("#pagePagination li").remove();
		console.log("error")
	}
	
}
function searchForArrivalWithPreviousData(part) {
	console.log(part)
	console.log("searchForArrivalWithPreviousData")
	$.ajax({
		url: `/User/SearchUpcomingArrivals/params?key=${key}&value=${value}`
	}).then(function (data) {
		console.log(data[0])
		console.log(`${part}`)
		createATableFromTheData(data, part, true)
	}).fail(
		function (err) {
			console.error(err)
		}
	)
}
function createATableFromTheData(data, part, isSearch) {
	//Table
	$("#tblUpcomingArrivals thead").remove();
	$("#tblUpcomingArrivals tbody").remove();
	var $flights_table = $("#tblUpcomingArrivals")
	$flights_table.append(`<thead class="thead-dark">
								<tr>
									<th scope="col">Flight ID</th>
									<th scope="col">Airline Company</th>
									<th scope="col">From</th>
									<th scope="col">To</th>
									<th scope="col">Arrival Time</th>
									<th scope="col">Status</th>
								</tr>
							</thead>
							<tbody>`);
	var now = new Date()
	for (var i = (part - 1) * 10; i < part * 10 & i < data.length; i++) {
		var arrivalDate = new Date(data[i].LandingTime)
		var diff = Math.floor((arrivalDate - now) / (1000 * 60));
		if (diff > 15 & diff < 120) {
			$flights_table.append(`<tr> <th scope="row">${data[i].ID}</th><td>${data[i].AirlineCompanyName}</td><td>${data[i].OriginCountryName}</td>
								<td>${data[i].DestinationCountryName}</td><td>${data[i].LandingTime}</td><td style="color:GoldenRod">Final</td></tr>`)
		}
		else if (diff >= 120) {
			$flights_table.append(`<tr> <th scope="row">${data[i].ID}</th><td>${data[i].AirlineCompanyName}</td><td>${data[i].OriginCountryName}</td>
								<td>${data[i].DestinationCountryName}</td><td>${data[i].LandingTime}</td><td style="color:green">Not Final</td></tr>`)
		}
		else if (diff <= 0) {
			$flights_table.append(`<tr> <th scope="row">${data[i].ID}</th><td>${data[i].AirlineCompanyName}</td><td>${data[i].OriginCountryName}</td>
								<td>${data[i].DestinationCountryName}</td><td>${data[i].LandingTime}</td><td style="color:red">Landed</td></tr>`)
		}
		else {
			$flights_table.append(`<tr> <th scope="row">${data[i].ID}</th><td>${data[i].AirlineCompanyName}</td><td>${data[i].OriginCountryName}</td>
								<td>${data[i].DestinationCountryName}</td><td>${data[i].LandingTime}</td><td style="color:orange">Landing</td></tr>`)
		}
	}
	$flights_table.append(`</tbody>`)
	//End Table

	//Pagination
	var $pagination = $("#pagePagination")
	$("#pagePagination li").remove();
	var size = Math.ceil(data.length / 10)
	if (part > 1) {
		if (isSearch) {
			$pagination.append(`
			<li class="page-item">
				<a class="page-link" href="#" aria-label="Previous" onclick="loadByPreviousSearch(${part - 1})" id="a">
                    <span aria-hidden="true">&laquo;</span>
                    <span class="sr-only">Previous</span>
                </a>
            </li>`)
		}
		else {
			$pagination.append(`
			<li class="page-item">
				<a class="page-link" href="#" aria-label="Previous" onclick="load(${part - 1})" id="a">
                    <span aria-hidden="true">&laquo;</span>
                    <span class="sr-only">Previous</span>
                </a>
            </li>`)
		}
	}
	var i = part - 2
	if (i < 1) {
		i = 1
	}
	for (; i <= size & i <= part + 2; i++) {
		if (isSearch) {
			$pagination.append(`
			<li class="page-item" onclick="loadByPreviousSearch(${i})">
				<a class="page-link" href="#">${i}</a>
            </li>`)
		}
		else {
			$pagination.append(`
			<li class="page-item" onclick="load(${i})">
				<a class="page-link" href="#">${i}</a>
            </li>`)
		}
	}
	if (size - part >= 1) {
		$pagination.append(``)
		if (isSearch) {
			$pagination.append(`
            <li class="page-item">
                <a class="page-link" href="#" aria-label="Next" onclick="loadByPreviousSearch(${part + 1})">
                    <span aria-hidden="true">&raquo;</span>
                    <span class="sr-only">Next</span>
                </a>
            </li>`)
		}
		else {
			$pagination.append(`
            <li class="page-item">
                <a class="page-link" href="#" aria-label="Next" onclick="load(${part + 1})">
                    <span aria-hidden="true">&raquo;</span>
                    <span class="sr-only">Next</span>
                </a>
            </li>`)
		}
	}
	//End Pagination
}
function createQuery(filter, search) {
	if (search == "") {
		console.log(`reached all query search is: ${search}`)
		return "all"
	}
	else {
		if (filter == 0 & !isNaN(search)) {
			console.log("0")
			return ["ID", search]
		}
		else if (filter == 1) {
			console.log("1")
			return ["AIRLINE_NAME", search]
		}
		else if (filter == 2) {
			console.log("2")
			return ["ORIGIN_COUNTRY", search]
		}
		else if (filter == 3) {
			console.log("3")
			return ["DESTINATION_COUNTRY", search]
		}
		else {
			return "error"
		}
	}
}