const baseAdress = "https://localhost:7021/";
var selectedUser = null;

async function createUser(username, balance, currencyType)
{
	var body = { username, balance: Number(balance), currencyType }

	const res = await fetch(baseAdress + "Users", {
		method: 'post',
		headers: {
		  'Content-Type': 'application/json',
		},
		body: JSON.stringify(body),
	  })

	if(res.ok)
	{
		alert("Клиент был добавлен!")
		return await res.json();
	}
	else
		alert("Ошибка добавления клиента!")
}

async function updateUser(id)
{
	var username = document.querySelector("#username").value;
	var balance = document.querySelector("#balance").value;
	var currencyType = document.querySelector("#selectCurrency").value;

	balance == '' ? balance = null : balance = Number(balance);
	username == '' ? username = null : username = username

	var body = { username, balance, currencyType }

	const res = await fetch(baseAdress + "Users/" + id, {
		method: 'put',
		headers: {
		  'Content-Type': 'application/json',
		},
		body: JSON.stringify(body),
	  })

	if(res.ok)
	{
		return true
	}
	else
		return false
}

async function deleteUser(id)
{
	const res = await fetch(baseAdress + 'Users/' + id, {method: 'delete'})

	if(res.ok)
	{
		return true;
	}
	else
	{
		return false;
	}
}

async function getUser(id)
{
	const res = await fetch(baseAdress + 'Users/' + id)

	if(res.ok)
	{
		return await res.json();
	}
}

async function loadCurrencyTypes()
{
	const res = await fetch(baseAdress + 'Transactions/CurrencyTypes')

	if(res.ok)
	{
		const types = await res.json();
		typesToHtml(types)
	}
	else
	{
		alert("Не удалось получить доступные валюты")
	}
}

async function loadAllUsers()
{
	const res = await fetch(baseAdress + 'Users/Search', {method: 'post'})

	if(res.ok)
	{
		const users = await res.json();
		users.forEach(user => {
			userToHtmlTable(user)
		});
	}
	else
	{
		alert("Не удалось загрузить клиентов!")
	}
}

function userToHtmlTable(user)
{
	const list = document.querySelector("#users-list")
	list.insertAdjacentHTML('beforeend', `                
	<tr>
		<td>${user.id}</td>
		<td>${user.username}</td>
		<td>${user.balance}</td>
		<td>${user.currencyType}</td>
		<td>
			<a href="#" class="btn btn-warning btn-sm edit edit">Edit</a>
			<a href="#" class="btn btn-danger btn-sm edit delete">Delete</a>
		</td>
	</tr>
`);
}

function typesToHtml(types)
{
	select = document.getElementById('selectCurrency')

	types.forEach(element => {
		select.appendChild(new Option(element, element))
	});
}

async function onButtonUpdateClick()
{
	if(selectedUser == null)
		alert("Выберите объект, нажав на кнопку edit рядом с ним")
	else
	{
		if(await updateUser(selectedUser.id) == true)
		{
			alert("Клиент отредактирован!")
			location.reload();
		}
		else
		{
			alert("Ошибка редактирования!")
		}
	}
	document.getElementById("updateButton").style.visibility = "hidden";
}

function loadUserToForm(user)
{
	document.getElementById('username').value = user.username
	document.getElementById('balance').value = user.balance
	document.getElementById('selectCurrency').value = user.currencyType
}

document.querySelector("#users-list").addEventListener("click", async (e) =>{
	target = e.target;
	if(target.classList.contains("delete")){
		var id = target.parentElement.parentElement.firstChild.nextSibling.textContent
		if(await deleteUser(id) == true)
		{
			target.parentElement.parentElement.remove();
			alert("Пользователь был удален!")
		}
		else
		{
			alert("Не получилось удалить пользователя!")
		}
	}
	else if(target.classList.contains("edit")){
		document.getElementById("updateButton").style.visibility = "visible";
		var id = target.parentElement.parentElement.firstChild.nextSibling.textContent
		selectedUser = await getUser(id)
		loadUserToForm(selectedUser)
	}
});

document.addEventListener("DOMContentLoaded", function(){
	loadCurrencyTypes();
	loadAllUsers(12);
})

document.querySelector("#user-form").addEventListener("submit", async (e) => {
	e.preventDefault()

	const username = document.querySelector("#username").value;
	const balance = document.querySelector("#balance").value;
	const currency = document.querySelector("#selectCurrency").value;

	if(username == "" || balance == "" || currency == "")
		alert("Все поля обязательны к заполнению!")

	else
	{
		var user = await createUser(username, balance, currency)
		userToHtmlTable(user)
	}
})