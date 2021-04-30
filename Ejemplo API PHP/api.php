<?php
	
	switch($_SERVER['REQUEST_METHOD']){
		case 'GET':
			if(isset($_GET['id'])){
				get_one($_GET['id']);
			}else{
				get_all();
			}
		break;
		
		case 'POST':
			post();
		break;
		
		case 'PUT':
			put();
		break;
		
		case 'DELETE':
			delete();
		break;
	}
	
	function get_all(){
		echo('Obtener todos los recursos');
	}
	
	function get_one($id){
		echo('Obtener el recurso con id:' . $id);
	}
	
	function post(){
		echo('Crear un nuevo recurso');
		echo(', Datos: ' . get_body());
	}
	
	function put(){
		echo('Actualizar un recurso');
		echo(', Datos: ' . get_body());
	}
	
	function delete(){
		echo('Borrar un recurso');
	}
	
	function get_body(){
		return file_get_contents('php://input');
	}
?>