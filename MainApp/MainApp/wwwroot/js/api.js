async function deletePart(id, table) {
    const response = await fetch("/api/part/remove/"+table+"&"+id, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok) {
        const partId = await response.json();
        document.getElementById(partId).remove();
    }
    else {
        const error = await response.json();
        console.log(error.message);
    }
}

