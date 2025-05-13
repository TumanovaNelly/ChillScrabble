document.addEventListener("DOMContentLoaded", () => {
    const boardCells = document.querySelectorAll(".board-cell");
    const playerSlots = document.querySelectorAll(".slot");
    
    boardCells.forEach((cell) => { toDraggable(cell); })
    playerSlots.forEach((slot) => { toDraggable(slot); })
});

document.addEventListener("DOMContentLoaded", () => {
    const readyButton = document.getElementById("ready-button");
    readyButton.addEventListener("click", () => { startMoving(0); });
});

function toDraggable(element) {
    element.addEventListener("dragover", (e) => {
        e.preventDefault();
    })

    element.addEventListener("drop", (e) => {
        e.preventDefault();

        const data = JSON.parse(e.dataTransfer.getData("text/plain"));
        const letter = data.letter;
    });
}




function startMoving(playerId) {
    fetch(`/Game/DealNewTiles?playerId=${playerId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Ошибка HTTP: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            if (data.success && data.tiles) {
                dealNewTiles(data.tiles, playerId);
            } else {
                console.error("Некорректный ответ сервера:", data);
            }
        })
        .catch(error => {
            console.error("Ошибка при получении данных с сервера:", error);
        });
}

// Функция для раздачи новых фишек
function dealNewTiles(tiles, playerId) {
    const emptySlots = document.querySelectorAll(`.slot.empty-slot[data-player='${playerId}']`);
    
    tiles.forEach((tile, index) => {
        // Проверяем, есть ли свободный слот
        if (index < emptySlots.length) {
            const slot = emptySlots[index];

            // Обновляем содержимое слота
            slot.innerHTML = `<span>${tile.Letter}</span>`;
            slot.classList.remove("empty-slot"); // Убираем класс "empty-slot"
            slot.classList.add("chip", "active-chip"); // Добавляем новые классы

            // Добавляем атрибуты для данных фишки
            slot.dataset.id = tile.Id;
            slot.dataset.letter = tile.Letter;
            slot.dataset.value = tile.Value;

            // Делаем слот перетаскиваемым
            slot.setAttribute("draggable", "true");

            // Добавляем обработчики событий Drag and Drop
            slot.addEventListener("dragstart", (e) => {
                e.dataTransfer.setData("text/plain", JSON.stringify({
                    id: e.target.dataset.id,
                    letter: e.target.dataset.letter,
                    value: e.target.dataset.value
                }));
                e.target.classList.add("dragging");
            });

            slot.addEventListener("dragend", (e) => {
                e.target.classList.remove("dragging");
            });
        }
    });
}

// Функция для обновления состояния draggable
function updateDraggableState() {
    const allTiles = document.querySelectorAll(".draggable");
    allTiles.forEach(tile => {
        // Проверяем условие (например, если фишка уже на игровом поле)
        const parentCell = tile.parentElement;
        if (parentCell && parentCell.classList.contains("board-cell")) {
            tile.classList.remove("draggable");
        }
    });
}


//
// 
//     const boardCells = document.querySelectorAll('.board-cell');
//     const emptyBoardCells = Array.from(boardCells).filter(cell => cell.classList.length === 1);
//    
//     const playerSlots = document.querySelectorAll('.slot');
//     const emptyPlayerCells = Array.from(playerSlots).filter(cell => cell.classList.contains('empty-slot'));
//     const activeChipPlayerCells = Array.from(playerSlots).filter(cell => cell.classList.contains('chip chip-active'));
//    
//    
//     const draggables = document.querySelectorAll(".draggable");
//     const droppables = document.querySelectorAll(".droppable");
//    
//
//     draggables.forEach(draggable => {
//         draggable.addEventListener("dragstart", (e) => {
//             e.dataTransfer.setData("text/plain", JSON.stringify({
//                 letter: e.target.dataset.letter,
//                 value: e.target.dataset.value
//             }));
//             e.target.classList.add("dragging");
//         });
//
//         draggable.addEventListener("dragend", (e) => {
//             e.target.classList.remove("dragging");
//         });
//     });
//
//
//     droppables.forEach(droppable => {
//         droppable.addEventListener("dragover", (e) => {
//             e.preventDefault();
//         });
//
//         droppable.addEventListener("drop", (e) => {
//             e.preventDefault();
//
//
//             // Проверяем, что ячейка пуста
//
//             console.log("Цель:", droppable);
//
//             const data = JSON.parse(e.dataTransfer.getData("text/plain"));
//             console.log("Перетаскиваемая фишка:", data);
//
//             const letter = data.letter;
//
//             const row = droppable.dataset.row; // Получаем строку
//             const col = droppable.dataset.col; // Получаем столбец
//            
//             // Отправляем данные на сервер
//             fetch('/Game/MoveTile', {
//                 method: 'POST',
//                 headers: {
//                     'Content-Type': 'application/json'
//                 },
//                 body: JSON.stringify({
//                     letter: letter,
//                     row: parseInt(row),
//                     col: parseInt(col)
//                 })
//             })
//                 .then(response => response.json())
//                 .then(data => {
//                     if (data.success) {
//                         // Изменяем класс ячейки
//                         droppable.classList.add("filled-cell"); // Добавляем новый класс
//                         droppable.classList.remove("droppable"); // Удаляем старый класс
//
//                         console.log("Цель:", droppable);
//
//                         // Добавляем букву в ячейку
//                         droppable.innerHTML = `<span>${letter}</span>`;
//
//                         const draggingElement = document.querySelector(".dragging");
//                         console.log(draggingElement);
//                         if (draggingElement) {
//                             // Заменяем содержимое на пустую ячейку
//                             draggingElement.innerHTML = ""; // Очищаем содержимое
//                             draggingElement.classList.remove("draggable"); // Убираем возможность перетаскивания
//                             draggingElement.classList.add("empty-slot"); // Добавляем класс для пустой ячейки
//                            
//                         }
//                     }
//                 });
//            
//         });
//     });
// });