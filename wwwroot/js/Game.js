document.addEventListener("DOMContentLoaded", () => {
    const draggables = document.querySelectorAll(".draggable-slot");
    const droppables = document.querySelectorAll(".droppable");

    console.log("Все droppable элементы:", droppables);
    // Обработчик начала перетаскивания
    draggables.forEach(draggable => {
        draggable.addEventListener("dragstart", (e) => {
            e.dataTransfer.setData("text/plain", JSON.stringify({
                letter: e.target.dataset.letter,
                value: e.target.dataset.value
            }));
            e.target.classList.add("dragging");
        });

        draggable.addEventListener("dragend", (e) => {
            e.target.classList.remove("dragging");
        });
    });

    // Обработчик для целевых областей
    droppables.forEach(droppable => {
        droppable.addEventListener("dragover", (e) => {
            e.preventDefault(); // Разрешаем перетаскивание
        });

        droppable.addEventListener("drop", (e) => {
            e.preventDefault();

            // Логируем данные о цели
            console.log("Цель:", droppable);

            // Получаем данные о перетаскиваемой фишке
            const data = JSON.parse(e.dataTransfer.getData("text/plain"));
            console.log("Перетаскиваемая фишка:", data);

            const letter = data.letter;

            // Проверяем, что ячейка пуста
            if (!droppable.textContent) {
                // Логируем данные о цели
                console.log("Цель:", droppable);

                // Получаем данные о перетаскиваемой фишке
                const data = JSON.parse(e.dataTransfer.getData("text/plain"));
                console.log("Перетаскиваемая фишка:", data);

                const letter = data.letter;
                // Отправляем данные на сервер
                fetch('/Game/MoveTile', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        letter: letter,
                        row: parseInt(row),
                        col: parseInt(col)
                    })
                })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            // Обновляем интерфейс
                            droppable.innerHTML = `<span>${letter}</span>`;
                            droppable.style.backgroundColor = "#ccc";

                            // Удаляем фишку из исходного слота
                            const draggingElement = document.querySelector(".dragging");
                            if (draggingElement) {
                                draggingElement.remove();
                            }
                        }
                    });
            }
        });
    });
});