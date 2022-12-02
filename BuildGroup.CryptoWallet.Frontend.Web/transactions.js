const baseAdress = "https://localhost:7021/";

document.addEventListener("DOMContentLoaded", function(){
	getTransactionTypes();
	getAllTransactions();
})

async function getUser(id)
{
	const res = await fetch(baseAdress + 'Users/' + id)

	if(res.ok)
	{
		return await res.json();
	}
	else
	{
		alert(`User ${id} not found`)
	}
}

async function deleteTransaction(id)
{
	const res = await fetch(baseAdress + 'Transactions/' + id, {method: 'delete'})

	if(res.ok)
	{
		return true;
	}
	else
	{
		return false;
	}
}

async function getTransactionTypes()
{
	const res = await fetch(baseAdress + 'Transactions/TransactionTypes')
	
	if(res.ok)
	{
		const types = await res.json();
		transactionTypesToHtml(types)
	}
	else
	{
		alert("Не удалось получить типы транзакций")
	}
}

async function getAllTransactions()
{
	const res = await fetch(baseAdress + 'Transactions/Search', {method: 'post'})

	if(res.ok)
	{
		const transactions = await res.json();
		transactions.forEach(transaction => {
			transactionToHtml(transaction)
		});
	}
	else
	{
		alert("Не удалось загрузить клиентов!")
	}
}

async function createTransaction(fromUserId, toUserId, amount, transactionType)
{
	var body = {fromUserId, toUserId, amount, transactionType}

	const res = await fetch(baseAdress + "Transactions", {
		method: 'post',
		headers: {
		  'Content-Type': 'application/json',
		},
		body: JSON.stringify(body),
	  })

	if(res.ok)
	{
		alert("Транзакция была добавлена!")
		return await res.json();
	}
	else
		alert("Ошибка добавления транзакции!")
}

function transactionToHtml(transaction)
{
	const list = document.querySelector("#transactions-list")

	list.insertAdjacentHTML('beforeend', `                
	<tr>
		<td>${transaction.id}</td>
		<td>${transaction.fromUserId}</td>
		<td>${transaction.toUserId}</td>
		<td>${transaction.amount}</td>
		<td>${transaction.currencyType}</td>
		<td>${transaction.transactionType}</td>
		<td>${transaction.date}</td>
		<td>
			<a href="#" class="btn btn-danger btn-sm edit delete">Delete</a>
		</td>
	</tr>
`);
}

function transactionTypesToHtml(types)
{
	select = document.getElementById('selectType')

	types.forEach(element => {
		select.appendChild(new Option(element, element))
	});
}

function validationTransaction(fromUser, toUser, amount)
{
	if(fromUser.currencyType != toUser.currencyType)
	{
		alert('Невозможно создать транзакцию, так как разные валюты!')
		return false;
	}
	else
	{
		var fromUserNewBalance = fromUser.balance - amount
		if(fromUserNewBalance < 0)
		{
			alert('Невозможно создать транзакцию, недостаточно средств на счете!')
			return false;
		}
		else
		{
			return true;
		}
	}
}

document.querySelector("#transactions-list").addEventListener("click", async (e) =>{
	target = e.target;
	if(target.classList.contains("delete")){
		var id = target.parentElement.parentElement.firstChild.nextSibling.textContent
		if(await deleteTransaction(id) == true)
		{
			target.parentElement.parentElement.remove();
			alert("Транзакция была удалена!")
		}
		else
		{
			alert("Не получилось удалить транзакцию!")
		}
	}
});


document.querySelector("#transaction-form").addEventListener("submit", async (e) => {
	e.preventDefault()

	const fromUserId = document.querySelector("#fromUserId").value;
	const toUserId = document.querySelector("#toUserId").value;7
	const amount = document.querySelector("#amount").value;
	const transactionType = document.querySelector("#selectType").value;


	if(fromUserId == "" || toUserId == "" || amount == "" || transactionType == "")
	{
		alert("Все поля обязательны к заполнению!")
	}
	else
	{
		var fromUser = await getUser(fromUserId)
		var toUser = await getUser(toUserId)

		if(validationTransaction(fromUser, toUser, amount) == true)
		{
			var transaction = await createTransaction(fromUserId, toUserId, amount, transactionType)
			transactionToHtml(transaction)
		}
	}
})
