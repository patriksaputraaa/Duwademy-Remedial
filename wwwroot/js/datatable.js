window.initializeDataTable = (tableId) => {
    if (!$.fn.DataTable.isDataTable(`#${tableId}`)) {
        table.DataTable();
        console.log("DataTable initialized successfully.");
    } else {
        console.log("DataTable already initialized.");
    }

    console.log("Initializing DataTable for:", tableId);
    const table = $(`#${tableId}`);
    if (table.length) {
        table.DataTable();
        console.log("DataTable initialized successfully.");
    } else {
        console.log("Table not found: ", tableId);
    }
};
