import { render, screen, fireEvent, act } from '@testing-library/react'
import '@testing-library/jest-dom/extend-expect'
import { TodoItems } from '../TodoItems'

import { useFetchTodoItems, usePutTodoItem } from '../../../hooks'
import { TodoItem } from '@/models/TodoItem'
import React from 'react'

jest.mock('react', () => {
  const actualReact = jest.requireActual('react')
  return {
    ...actualReact,
    useState: jest.fn(),
  }
})
jest.mock('../../../hooks', () => ({
  useFetchTodoItems: jest.fn(),
  usePutTodoItem: jest.fn(),
}))

// const fetchTodoItemsFn = jest.fn(useFetchTodoItems)
// const putTodoItemFn = jest.fn(usePutTodoItem)

describe('TodoItems', () => {
  test('renders the todo items correctly', () => {
    const mockTodoItems: TodoItem[] = [
      { id: '1', description: 'Task 1', isCompleted: false },
      { id: '2', description: 'Task 2', isCompleted: false },
    ]
    const mockFetchData = jest.fn()
    const mockPutData = jest.fn()
    const useStateMock = (init: any) => [mockTodoItems, jest.fn()]

    useFetchTodoItems.mockReturnValue({
      data: mockTodoItems,
      loading: false,
      error: null,
      fetchData: mockFetchData,
    })

    usePutTodoItem.mockReturnValue({
      data: '1',
      loading: false,
      error: null,
      putData: mockPutData,
    })

    jest.spyOn(React, 'useState').mockImplementation(useStateMock)
    render(<TodoItems />)

    expect(screen.getByText('Showing 2 Item(s)')).toBeInTheDocument()
    expect(screen.getByText('Task 1')).toBeInTheDocument()
    expect(screen.getByText('Task 2')).toBeInTheDocument()
  })

  test('calls putData when "Mark as completed" button is clicked', async () => {
    const mockTodoItems: TodoItem[] = [
      { id: '1', description: 'Task 1', isCompleted: false },
      { id: '2', description: 'Task 2', isCompleted: false },
    ]
    const mockFetchData = jest.fn()
    const mockPutData = jest.fn()
    const useStateMock = () => [mockTodoItems, jest.fn()];

    useFetchTodoItems.mockReturnValue({
      data: mockTodoItems,
      loading: false,
      error: null,
      fetchData: mockFetchData,
    })

    usePutTodoItem.mockReturnValue({
      data: '1',
      loading: false,
      error: null,
      putData: mockPutData,
    })

    jest.spyOn(React, 'useState').mockImplementation(useStateMock);

    render(<TodoItems />)

    const markAsCompletedButtons = screen.getAllByText('Mark as completed')
    fireEvent.click(markAsCompletedButtons[0])
    render(<TodoItems />)
    await act(async () => {
      expect(screen.getByText('completed')).toBeInTheDocument()
    })
  })
})
