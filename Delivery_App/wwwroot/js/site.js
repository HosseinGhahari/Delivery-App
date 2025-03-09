document.addEventListener("DOMContentLoaded", function () {

    const searchInput = document.getElementById("searchInput");
    const tableBody = document.querySelector("tbody"); // Select the table body

    if (!searchInput || !tableBody) return; // Stop if elements are missing

    searchInput.addEventListener("input", async function () {
        let query = searchInput.value.trim();

        // If input is empty, reload the original page (shows all deliveries)
        if (query === "") {
            location.reload();
            return;
        }

        try {
            // Fetch filtered search results
            let response = await fetch(`/Index?handler=Search&search=${encodeURIComponent(query)}`);
            let data = await response.json();

            // Clear existing table rows
            tableBody.innerHTML = "";

            // If no results found, show an empty row
            if (data.length === 0) {
                tableBody.innerHTML = "<tr><td colspan='6' class='text-center'>نتیجه‌ای یافت نشد</td></tr>";
                return;
            }

            // Loop through search results and create new rows
            data.forEach(item => {
                let row = document.createElement("tr");
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
                tableBody.appendChild(row);
            });

        } catch (error) {
            console.error("Error fetching data:", error);
        }
    });
});
