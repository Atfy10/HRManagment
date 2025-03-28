const salaryInput = document.getElementById("Salary");
const positionSelect = document.getElementById("Position");


positionSelect.addEventListener("change", async () => {
    try {
        var result = await fetch("http://localhost:5093/Employee/PositionSalary/?position=" + positionSelect.value);
        let salary = await result.json();
        salaryInput.max = salary;
    } catch (error) {
        console.error("Error fetching salary:", error);
    }
});
