#!/bin/bash

# Ruta a tu base de datos
DB_PATH="./gymplanner.db"

if [ -f "$DB_PATH" ]; then
    echo "Eliminando la base de datos existente..."
    rm "$DB_PATH"
    echo "Base de datos eliminada."
else
    echo "No se encontró la base de datos en $DB_PATH"
fi

echo "Listo. Ahora podés ejecutar tu aplicación y se recreará la base de datos automáticamente."
