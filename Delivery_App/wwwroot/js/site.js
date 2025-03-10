
document.addEventListener("DOMContentLoaded", function () {

    const searchInput = document.getElementById("searchInput");
    const tableBody = document.querySelector("tbody"); // Select the table body
    const searchType = searchInput?.dataset.searchType; // Read search type from HTML

    if (!searchInput || !tableBody) return; // Stop if elements are missing

    searchInput.addEventListener("input", async function () {
        let query = searchInput.value.trim();

        // If input is empty, reload the original page (shows all data)
        if (query === "") {
            location.reload();
            return;
        }

        try {
            // Choose the correct handler based on search type
            let baseUrl = searchType === "Destination" ? "/Destination/Index" : "/Index"; 
            let handlerName = searchType === "Destination" ? "DestinationSearch" : "DeliverySearch";
            let url = `${baseUrl}?handler=${handlerName}&search=${encodeURIComponent(query)}`;

            let response = await fetch(url);
            let data = await response.json();

            // Clear the table before showing results
            tableBody.innerHTML = "";

            // Handle empty results
            if (data.length === 0) {
                tableBody.innerHTML = "<tr><td colspan='6' class='text-center'>نتیجه‌ای یافت نشد</td></tr>";
                return;
            }

            // Dynamically create table rows based on search type
            data.forEach(item => {
                let row = document.createElement("tr");

                if (searchType === "Destination") {
                    row.innerHTML = `
                        <th>${item.destinationName}</th>
                        <th>${item.price}</th>
                        <th><a href="/Destination/EditDestination?id=${item.id}">ویرایش</a></th>
                    `;
                } else {
                    row.innerHTML = `
                        <th>${item.destination}</th>
                        <th>${item.optionalPrice ? item.optionalPrice : item.price}</th>
                        <th>${item.persianDeliveryTime}</th>
                        <th class="${item.isPaid ? 'alert-success' : 'alert-danger'}">
                            ${item.isPaid ? 'پرداخت شد' : 'پرداخت نشد'}
                        </th>
                        <th><a href="/Delivery/EditDelivery?id=${item.id}">ویرایش</a></th>
                        <th><a href="/Index?handler=Remove&id=${item.id}" class="link-danger">حذف</a></th>
                    `;
                }
                tableBody.appendChild(row);
            });

        } catch (error) {
            console.error("Error fetching data:", error);
        }
    });
});
