-- DROP FUNCTION cmb_mostrararticulos();
-- DROP TYPE type_articulos_almacen;

CREATE TYPE type_articulos_almacen AS 
(
	Descripcion VARCHAR(200),
	Identificador INTEGER
);

CREATE OR REPLACE FUNCTION cmb_mostrararticulos()
RETURNS SETOF type_articulos_almacen AS
$$

	DECLARE
		reg RECORD;

	BEGIN
	
		FOR reg IN  SELECT CONCAT( b.numero_categoria, '-', LPAD(a.folio_articulo::TEXT,4,'0'), ' | ', a.nombre_articulo )::VARCHAR(200) AS descripcion, a.id AS Identificador
					FROM tb_articulosalmacen AS a
					JOIN tb_categorias AS b ON (a.numero_categoria_id = b.id)
					WHERE a.status = 0 
					ORDER BY b.numero_categoria, a.folio_articulo
		LOOP
			RETURN NEXT reg;
		END LOOP;
		RETURN;
	
	END

$$
LANGUAGE 'plpgsql';
